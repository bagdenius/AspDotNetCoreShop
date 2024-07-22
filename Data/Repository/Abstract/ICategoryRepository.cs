using Models;

namespace Data.Repository.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        new void Update(Category category);
    }
}
