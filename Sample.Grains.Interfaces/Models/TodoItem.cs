using Orleans.Concurrency;
using System;

namespace Sample.Grains.Models
{
    [Immutable]
    public class TodoItem
    {
        public TodoItem(Guid key, string title, bool isDone, Guid ownerKey)
        {
            Key = key;
            Title = title;
            IsDone = isDone;
            OwnerKey = ownerKey;
        }

        public Guid Key { get; }
        public string Title { get; }
        public bool IsDone { get; }
        public Guid OwnerKey { get; }

        public TodoItem WithIsDone(bool isDone) =>
            new TodoItem(Key, Title, isDone, OwnerKey);

        public TodoItem WithTitle(string title) =>
            new TodoItem(Key, title, IsDone, OwnerKey);
    }
}