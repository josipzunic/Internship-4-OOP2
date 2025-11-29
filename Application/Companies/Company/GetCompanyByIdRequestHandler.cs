using Application.Common.Model;
using Application.Common.Model.Responses.Companies;
using Application.DTOs.Companies;
using Domain.Common.Validation;
using Domain.Persistence.Companies;
using Domain.Persistence.User;

namespace Application.Companies;

public class GetCompanyByIdRequest
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class GetCompanyByIdRequestHandler : RequestHandler<GetCompanyByIdRequest, GetCompanyByIdResponse>
{
    private readonly ICompanyUnitOfWork _companyUnitOfWork;
    private readonly IUserUnitOfWork _userUnitOfWork;
    protected override async Task<Result<GetCompanyByIdResponse>> HandleRequest(GetCompanyByIdRequest request, Result<GetCompanyByIdResponse> result)
    {
        var user = await _userUnitOfWork.Repository.GetByUsername(request.Username);
        
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
        
        var company = await _companyUnitOfWork.Repository.GetById(request.Id);
        
        if (company == null)
        {
            var validationResult = new ValidationResult();
            validationResult.AddValidationItems(new ValidationItem
            {
                Code = "{CompanyNotFound}",
                Message = "Kompanija s danim id-om ne postoji",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BusinessRule
            });

            result.SetValidationResult(validationResult);
            return result;
        }

        var dtoCompany = new CompanyDto
        {
            Id = company.Id,
            CompanyName = company.CompanyName,
        };
        
        result.SetResult(new GetCompanyByIdResponse {Company = dtoCompany});
        return result;
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}