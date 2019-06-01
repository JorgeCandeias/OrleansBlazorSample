using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.Concurrency;
using Sample.Grains;
using Sample.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Buffers;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sample.Silo.Api
{
    [ApiController]
    [Route("api/todo")]
    public class TodoController : ControllerBase
    {
        private readonly IGrainFactory factory;

        public TodoController(IGrainFactory factory)
        {
            this.factory = factory;
        }

        [HttpGet]
        [SwaggerOperation("todo-get")]
        public async Task<TodoItem> GetAsync([Required] Guid itemKey)
        {
            var result = await factory.GetGrain<ITodoGrain>(itemKey).GetAsync();
            return result.Value;
        }

        [HttpGet("list")]
        [SwaggerOperation("todo-list")]
        public async Task<ImmutableArray<TodoItem>> ListAsync(Guid ownerKey)
        {
            // get all item keys for this owner
            var keys = await factory.GetGrain<ITodoManagerGrain>(ownerKey).GetAllAsync();

            // fast path for empty owner
            if (keys.Length == 0) return ImmutableArray<TodoItem>.Empty;

            // fan out and get all individual items in parallel
            var tasks = ArrayPool<Task<Immutable<TodoItem>>>.Shared.Rent(keys.Length);
            try
            {
                // issue all requests at the same time
                for (var i = 0; i < keys.Length; ++i)
                {
                    tasks[i] = factory.GetGrain<ITodoGrain>(keys[i]).GetAsync();
                }

                // compose the result as requests complete
                var result = ImmutableArray.CreateBuilder<TodoItem>(tasks.Length);
                for (var i = 0; i < keys.Length; ++i)
                {
                    result.Add((await tasks[i]).Value);
                }
                return result.ToImmutable();
            }
            finally
            {
                ArrayPool<Task<Immutable<TodoItem>>>.Shared.Return(tasks);
            }
        }

        public class PostModel
        {
            [Required]
            public Guid Key { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            public bool IsDone { get; set; }

            [Required]
            public Guid OwnerKey { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PostModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new TodoItem
            {
                Key = model.Key,
                Title = model.Title,
                IsDone = model.IsDone,
                OwnerKey = model.OwnerKey
            };

            await factory.GetGrain<ITodoGrain>(item.Key)
                .SetAsync(item.AsImmutable());

            return Ok();
        }
    }
}