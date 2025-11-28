namespace Domain.Persistence.Companies;

public interface ICompanyUnitOfWork
{
    ICompanyRepository Repository { get; }
}