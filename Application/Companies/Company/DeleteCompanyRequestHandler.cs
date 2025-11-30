using Application.Common.Model;
using Domain.Common.Validation;
using Domain.Persistence.Companies;
using Domain.Persistence.User;

namespace Application.Companies;

public class DeleteCompanyRequest
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class DeleteCompanyRequestHandler : RequestHandler<DeleteCompanyRequest, SuccessPostResponse>
{
    private readonly ICompanyUnitOfWork _companyUnitOfWork;
    private readonly IUserUnitOfWork _userUnitOfWork;

    public DeleteCompanyRequestHandler(ICompanyUnitOfWork companyUnitOfWork, IUserUnitOfWork userUnitOfWork)
    {
        _companyUnitOfWork = companyUnitOfWork;
        _userUnitOfWork = userUnitOfWork;
    }
    protected override async Task<Result<SuccessPostResponse>> HandleRequest(DeleteCompanyRequest request, Result<SuccessPostResponse> result)
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
        
        await _companyUnitOfWork.Repository.DeleteAsync(company.Id);
        await _companyUnitOfWork.SaveAsync();
        
        result.SetResult(new SuccessPostResponse(company.Id));
        return result;
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}