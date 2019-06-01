using Orleans;
using Orleans.Concurrency;
using Sample.Grains;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.ServerSide.Services
{
    public class TodoService
    {
        private readonly IClusterClient client;

        public TodoService(IClusterClient client)
        {
            this.client = client;
        }

        public async Task<IList<TodoItem>> GetAllAsync(Guid ownerKey)
        {
            // get all the todo item keys for this owner
            var itemKeys = await client.GetGrain<ITodoManagerGrain>(ownerKey)
                .GetAllAsync();

            // fan out to get the individual items from the cluster
            var tasks = new List<Task<Immutable<TodoItem>>>(itemKeys.Count);
            foreach (var itemKey in itemKeys)
            {
                tasks.Add(client.GetGrain<ITodoGrain>(itemKey).GetAsync());
            }

            // gather all results as they come
            var list = new List<TodoItem>(itemKeys.Count);
            foreach (var task in tasks)
            {
                list.Add((await task).Value);
            }

            return list;
        }

        public Task SetAsync(TodoItem item) =>
            client.GetGrain<ITodoGrain>(item.Key).SetAsync(item.AsImmutable());
    }
}