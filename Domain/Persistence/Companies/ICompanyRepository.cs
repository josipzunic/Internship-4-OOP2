using System.Threading.Tasks;
using Domain.Entities.Companies;
using Domain.Persistence.Common;

namespace Domain.Persistence.Companies;

public interface ICompanyRepository : IRepository<Company, int>
{
    Task<Company> GetById(int id);
}