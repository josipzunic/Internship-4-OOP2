using Domain.Persistence.Companies;
using Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Company;

public class CompanyRepository : Repository<Domain.Entities.Companies.Company, int>, ICompanyRepository
{
    private readonly CompaniesDbContext _dbContext;
    private readonly IDapperManager _dapperManager;
    public CompanyRepository(CompaniesDbContext dbContext, IDapperManager dapperManager) : base(dbContext)
    {
        _dapperManager = dapperManager;
        _dbContext = dbContext;
    }

    public async Task<Domain.Entities.Companies.Company> GetById(int id)
    {
        return await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Domain.Entities.Companies.Company> GetByCompanyName(string companyName)
    {
        return await _dbContext.Companies.FirstOrDefaultAsync(c => c.CompanyName == companyName);
    }

    public async Task<List<Domain.Entities.Companies.Company>> GetAllCompanies()
    {
        return await _dbContext.Companies.ToListAsync();
    }
}