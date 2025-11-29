using Domain.Persistence.Common;

namespace Domain.Persistence.Companies;

public interface ICompanyUnitOfWork : IUnitOfWork
{
    ICompanyRepository Repository { get; }
}