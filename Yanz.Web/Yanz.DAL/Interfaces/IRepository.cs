using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yanz.DAL.Interfaces
{
    public interface IRepository<T> where T: class
    {
        T Get(string id);
        Task<T> GetAsync(string id);

        void Add(T item);
        void AddRange(IEnumerable<T> entities);

        Task AddAsync(T item);
        Task AddRangeAsync(IEnumerable<T> entities);

        void Update(T item);
        void UpdateRange(IEnumerable<T> entities);

        void Remove(T item);
        void RemoveRange(IEnumerable<T> entities);
    }
}
