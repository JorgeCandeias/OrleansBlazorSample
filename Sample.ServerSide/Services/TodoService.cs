using Orleans;
using Orleans.Concurrency;
using Sample.Grains;
using Sample.Models;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public async Task<ImmutableArray<TodoItem>> GetAllAsync(Guid ownerKey)
        {
            // get all the todo item keys for this owner
            var itemKeys = await client.GetGrain<ITodoManagerGrain>(ownerKey)
                .GetAllAsync();

            // fan out to get the individual items from the cluster in parallel
            var tasks = ArrayPool<Task<Immutable<TodoItem>>>.Shared.Rent(itemKeys.Length);
            try
            {
                // issue all individual requests at the same time
                for (var i = 0; i < itemKeys.Length; ++i)
                {
                    tasks[i] = client.GetGrain<ITodoGrain>(itemKeys[i]).GetAsync();
                }

                // build the result as requests complete
                var result = ImmutableArray.CreateBuilder<TodoItem>(itemKeys.Length);
                for(var i = 0; i < itemKeys.Length; ++i)
                {
                    var item = await tasks[i];
                    result.Add(item.Value);
                }
                return result.ToImmutable();
            }
            finally
            {
                ArrayPool<Task<Immutable<TodoItem>>>.Shared.Return(tasks);
            }
        }

        public Task SetAsync(TodoItem item) =>
            client.GetGrain<ITodoGrain>(item.Key).SetAsync(item.AsImmutable());
    }
}