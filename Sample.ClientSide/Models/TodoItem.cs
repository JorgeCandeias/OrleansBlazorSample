using System;

namespace Sample.ClientSide.Models
{
    public class TodoItem : IEquatable<TodoItem>
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public Guid OwnerKey { get; set; }

        public bool Equals(TodoItem other) =>
            Key == other.Key &&
            OwnerKey == other.OwnerKey &&
            Title == other.Title &&
            IsDone == other.IsDone;

        public override int GetHashCode() =>
            HashCode.Combine(Key, Title, IsDone, OwnerKey);
    }
}