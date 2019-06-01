using Orleans;
using Orleans.Concurrency;
using Orleans.Runtime;
using Sample.Models;
using System;
using System.Threading.Tasks;

namespace Sample.Grains
{
    public class TodoGrain : Grain, ITodoGrain
    {
        private readonly IPersistentState<State> state;

        private Guid GrainKey => this.GetPrimaryKey();

        public TodoGrain([PersistentState("State")] IPersistentState<State> state)
        {
            this.state = state;
        }

        public Task<Immutable<TodoItem>> GetAsync() => Task.FromResult(state.State.Item.AsImmutable());

        public async Task SetAsync(Immutable<TodoItem> item)
        {
            // ensure the key is consistent
            if (item.Value.Key != GrainKey)
            {
                throw new InvalidOperationException();
            }

            // save the item
            state.State.Item = item.Value;
            await state.WriteStateAsync();

            // register the item with its owner list
            await GrainFactory.GetGrain<ITodoManagerGrain>(item.Value.OwnerKey)
                .RegisterAsync(item.Value.Key);
        }

        public class State
        {
            public TodoItem Item { get; set; }
        }
    }
}