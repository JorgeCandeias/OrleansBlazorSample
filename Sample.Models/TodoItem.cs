using System;

namespace Sample.Models
{
    public class TodoItem
    {
        public Guid Key { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public Guid OwnerKey { get; set; }
    }
}