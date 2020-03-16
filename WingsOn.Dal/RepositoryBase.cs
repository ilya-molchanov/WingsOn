using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WingsOn.Domain;

namespace WingsOn.Dal
{
    public class RepositoryBase<T> : IRepository<T> where T : DomainObject
    {
        protected RepositoryBase()
        {
            Repository = new List<T>();
        }

        protected List<T> Repository;

        public IEnumerable<T> GetAll()
        {
            return Repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var repository = await Task.FromResult<IEnumerable<T>>(Repository);
            return repository;
        }

        public T Get(int id)
        {
            return GetAll().SingleOrDefault(a => a.Id == id);
        }

        public async Task<T> GetAsync(int id)
        {
            var entity = (await GetAllAsync())
                .SingleOrDefault(a => a.Id == id);
            return entity;
        }

        public void Save(T element)
        {
            if (element == null)
            {
                return;
            }

            T existing = Get(element.Id);
            if (existing != null)
            {
                Repository.Remove(existing);
            }

            Repository.Add(element);
        }

        public async Task<T> SaveAsync(T element)
        {
            if (element == null)
            {
                return null;
            }

            T existing = await GetAsync(element.Id);
            if (existing != null)
            {
                Repository.Remove(existing);
            }

            Repository.Add(element);

            return await Task.FromResult(element); 
        }
    }
}
