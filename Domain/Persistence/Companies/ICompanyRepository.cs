using System.Threading.Tasks;
using Domain.Entities.Companies;
using Domain.Persistence.Common;
using System.Collections.Generic;
namespace Domain.Persistence.Companies;

public interface ICompanyRepository : IRepository<Company, int>
{
    Task<Company> GetById(int id);
    Task<Company> GetByCompanyName(string companyName);
    Task<List<Company>> GetAllCompanies();
}