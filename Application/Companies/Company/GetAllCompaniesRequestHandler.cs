using Application.Common.Model;
using Application.Common.Model.Responses.Companies;
using Application.DTOs.Companies;
using Application.Users.User;
using Domain.Common.Validation;
using Domain.Persistence.Companies;
using Domain.Persistence.User;

namespace Application.Companies;

public class GetAllCompaniesRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class GetAllCompaniesRequestHandler : RequestHandler<GetAllCompaniesRequest, GetAllCompanyResponse>
{
    private readonly ICompanyUnitOfWork _companyUnitOfWork;
    private readonly IUserUnitOfWork _userUnitOfWork;

    public GetAllCompaniesRequestHandler(ICompanyUnitOfWork companyUnitOfWork, IUserUnitOfWork userUnitOfWork)
    {
        _companyUnitOfWork =  companyUnitOfWork;
        _userUnitOfWork = userUnitOfWork;
    }
    protected override async Task<Result<GetAllCompanyResponse>> HandleRequest(GetAllCompaniesRequest request, Result<GetAllCompanyResponse> result)
    {
        var user = await _userUnitOfWork.Repository.GetByUsername(request.Username);
        var companies = await _companyUnitOfWork.Repository.GetAllCompanies();
        
        if (user == null || !user.IsActive || user.Password != request.Password)
        {
            var authValidationResult = new ValidationResult();
            authValidationResult.AddValidationItems(new ValidationItem
            {
                Code = "{AnauthorizedUser}",
                Message = "Kriva lozinka ili korisničnko ime ili korisnik nije aktivan",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BusinessRule
            });
            result.SetValidationResult(authValidationResult);
            return result;
        }
        
        if (companies == null || !companies.Any())
        {
            var validationResult = new ValidationResult();
            validationResult.AddValidationItems(new ValidationItem
            {
                Code = "{CompaniesEmpty}",
                Message = "Nema kompanija u bazi podataka",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BusinessRule
            });

            result.SetValidationResult(validationResult);
            return result;
        }

        var dtoList = companies.Select(c => new CompanyDto
        {
            Id = c.Id,
            CompanyName = c.CompanyName,
        }).ToList();
        
        result.SetResult(new GetAllCompanyResponse(){Companies = dtoList});
        return result;
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}