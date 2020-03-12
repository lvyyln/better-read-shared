using System.Threading.Tasks;
using BetterRead.Shared.Domain.Search;

namespace BetterRead.Shared.Services.Abstractions
{
    public interface IFetchService
    {
        Task<Result[]> GetDataAsync(string searchTerm);
    }
}