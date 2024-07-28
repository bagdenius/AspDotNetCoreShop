using Models;

namespace Data.Repository.Abstract
{
    public interface ICompanyRepository : IRepository<Company>
    {
        new void Update(Company company);
    }
}
