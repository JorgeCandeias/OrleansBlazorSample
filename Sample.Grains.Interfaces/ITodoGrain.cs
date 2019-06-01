using Orleans;
using Orleans.Concurrency;
using Sample.Models;
using System.Threading.Tasks;

namespace Sample.Grains
{
    public interface ITodoGrain : IGrainWithGuidKey
    {
        Task SetAsync(Immutable<TodoItem> item);
        Task ClearAsync();
        Task<Immutable<TodoItem>> GetAsync();
    }
}