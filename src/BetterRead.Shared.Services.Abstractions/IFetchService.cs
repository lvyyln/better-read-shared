using System.Collections.Generic;
using System.Threading.Tasks;
using QuickType;

namespace BetterRead.Shared.Services.Abstractions
{
    public interface IFetchService
    {
        Task<List<Result>> GetDataAsync(string searchTerm, string address);
    }
}