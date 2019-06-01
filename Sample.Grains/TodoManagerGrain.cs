using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Sample.Grains
{
    public class TodoManagerGrain : Grain, ITodoManagerGrain
    {
        private readonly IPersistentState<State> state;
        private readonly HashSet<Guid> keys = new HashSet<Guid>();

        public TodoManagerGrain([PersistentState("State")] IPersistentState<State> state)
        {
            this.state = state;
        }

        public override Task OnActivateAsync()
        {
            if (state.State.Items == null)
            {
                state.State.Items = new LinkedList<Guid>();
            }

            return base.OnActivateAsync();
        }

        public Task RegisterAsync(Guid itemKey)
        {
            if (keys.Contains(itemKey)) return Task.CompletedTask;

            keys.Add(itemKey);
            state.State.Items.AddLast(itemKey);
            return state.WriteStateAsync();
        }

        public Task<ImmutableList<Guid>> GetAllAsync() =>
            Task.FromResult(state.State.Items.ToImmutableList());

        public class State
        {
            public LinkedList<Guid> Items { get; set; }
        }
    }
}