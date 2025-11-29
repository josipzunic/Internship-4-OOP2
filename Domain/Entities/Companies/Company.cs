using System.Threading.Tasks;
using Domain.Common.Model;
using Domain.Common.Validation;
using Domain.Common.Validation.ValidationItems;
using Domain.Persistence.Companies;

namespace Domain.Entities.Companies;

public class Company
{
    public const int MaxCompanyNameLength = 150;
    
    public int Id { get; set; }
    public string CompanyName { get; set; }

    public async Task<Result<bool>> Create(ICompanyRepository companyRepository)
    {
        ValidationResult validationResult = await CreateOrUpdateValidation();
        if (validationResult.HasErrors)
            return new Result<bool>(false, validationResult);

        await companyRepository.InsertAsync(this);
        
        return new Result<bool>(true, validationResult);
    }

    public async Task<ValidationResult> CreateOrUpdateValidation()
    {
        var validationResult = new ValidationResult();
        
        if (string.IsNullOrWhiteSpace(CompanyName))
            validationResult.AddValidationItems(ValidationItems.Company.CompanyNameRequired);
        else
            if (CompanyName.Length > MaxCompanyNameLength)
                validationResult.AddValidationItems(ValidationItems.Company.MaxCompanyNameLength);

        return validationResult;
    }
}