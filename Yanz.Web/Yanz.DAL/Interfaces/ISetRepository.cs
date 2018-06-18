using System.Collections.Generic;
using System.Threading.Tasks;
using Yanz.DAL.Entities;

namespace Yanz.DAL.Interfaces
{
    public interface ISetRepository : IRepository<Set>
    {
        Task<IEnumerable<Set>> GetAllWithQuestions(string userId);
        Task<Set> GetByUserAsync(string id, string userId);
    }
}
