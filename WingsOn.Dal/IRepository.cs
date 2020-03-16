using System.Collections.Generic;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Dal
{
    public interface IRepository<T> where T : DomainObject
    {
        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync();

        T Get(int id);

        Task<T> GetAsync(int id);

        void Save(T element);

        Task<T> SaveAsync(T element);
    }
}
