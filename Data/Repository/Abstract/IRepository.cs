using System.Linq.Expressions;

namespace Data.Repository.Abstract
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(string? includeProperties = null);
        abstract T Get(string id, string? includeProperties = null, bool tracked = false);
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Save();
    }
}
