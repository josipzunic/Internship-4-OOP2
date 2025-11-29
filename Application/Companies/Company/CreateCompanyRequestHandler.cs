using Application.Common.Model;
using Domain.Common.Validation;
using Domain.Common.Validation.ValidationItems;
using Domain.Entities.Companies;
using Domain.Persistence.Companies;
using Domain.Persistence.User;

namespace Application.Companies;

public class CreateCompanyRequest
{
    public string CompanyName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class CreateCompanyRequestHandler : RequestHandler<CreateCompanyRequest, SuccessPostResponse>
{
    private readonly ICompanyUnitOfWork _companyUnitOfWork;
    private readonly IUserUnitOfWork _userUnitOfWork;
    protected async override Task<Result<SuccessPostResponse>> HandleRequest(CreateCompanyRequest request, Result<SuccessPostResponse> result)
    {
        var user = await _userUnitOfWork.Repository.GetByUsername(request.Username);
        
        var company = new Company
        {
            CompanyName = request.CompanyName
        };

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

        var validationResult = await company.Create(_companyUnitOfWork.Repository);
        
        var existingCompanyName = await _companyUnitOfWork.Repository.GetByCompanyName(company.CompanyName);
        if (existingCompanyName != null && existingCompanyName.Id != company.Id)
            validationResult.ValidationResult.AddValidationItems(ValidationItems.Company.CompanyNameUnique);
        
        result.SetValidationResult(validationResult.ValidationResult);

        if (result.HasErrors)
            return result;

        await _companyUnitOfWork.Repository.InsertAsync(company);
        await _companyUnitOfWork.SaveAsync();
        
        result.SetResult(new SuccessPostResponse(company.Id));
        return result;
        
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}