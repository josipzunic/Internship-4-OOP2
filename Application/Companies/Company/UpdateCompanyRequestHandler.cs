using Application.Common.Model;
using Domain.Common.Validation;
using Domain.Common.Validation.ValidationItems;
using Domain.Persistence.Companies;
using Domain.Persistence.User;

namespace Application.Companies;

public class UpdateCompanyRequest
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class UpdateCompanyRequestHandler : RequestHandler<UpdateCompanyRequest, SuccessPostResponse>
{
    private readonly ICompanyUnitOfWork _companyUnitOfWork;
    private readonly IUserUnitOfWork _userUnitOfWork;
    protected async override Task<Result<SuccessPostResponse>> HandleRequest(UpdateCompanyRequest request, Result<SuccessPostResponse> result)
    {
        var company = await _companyUnitOfWork.Repository.GetById(request.Id);
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

        if (company == null)
        {
            var existsValidationResult = new ValidationResult();
            existsValidationResult.AddValidationItems(new ValidationItem
            {
                Code = "{CompanyNotFound}",
                Message = "Kompanija s danim id-om nije pronađena",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BusinessRule
            });
            result.SetValidationResult(existsValidationResult);
            return result;
        }

        company.CompanyName = request.CompanyName;

        var validationResult = await company.CreateOrUpdateValidation();

        var existingCompanyName =
            await _companyUnitOfWork.Repository.GetByCompanyName(request.CompanyName);
        if (existingCompanyName != null && existingCompanyName.Id != company.Id)
            validationResult.AddValidationItems(ValidationItems.Company.CompanyNameUnique);
        
        result.SetValidationResult(validationResult);
        if (validationResult.HasErrors)
            return result;
        
        _companyUnitOfWork.Repository.Update(company);
        await _companyUnitOfWork.SaveAsync();
        
        result.SetResult(new SuccessPostResponse(company.Id));
        return result;

    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}