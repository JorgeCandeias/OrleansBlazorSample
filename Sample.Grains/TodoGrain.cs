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

            // notify listeners - best effort only
            GetStreamProvider("SMS").GetStream<TodoNotification>(item.Value.OwnerKey, nameof(ITodoGrain))
                .OnNextAsync(new TodoNotification(item.Value.Key, item.Value))
                .Ignore();
        }

        public async Task ClearAsync()
        {
            if (state.State.Item == null) return;

            // hold on to the keys
            var itemKey = state.State.Item.Key;
            var ownerKey = state.State.Item.OwnerKey;

            // clear the state
            await state.ClearStateAsync();

            // notify listeners - best effort only
            GetStreamProvider("SMS").GetStream<TodoNotification>(ownerKey, nameof(ITodoGrain))
                .OnNextAsync(new TodoNotification(itemKey, null))
                .Ignore();

            // no need to stay alive anymore
            DeactivateOnIdle();
        }

        public class State
        {
            public TodoItem Item { get; set; }
        }
    }
}