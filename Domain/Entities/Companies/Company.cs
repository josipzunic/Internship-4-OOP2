using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Companies;

public class Company
{
    public const int MaxCompanyNameLength = 150;
    
    public string Id { get; set; }
    public string Name { get; set; }

    public async Task<Result<int>> Create()
    {
        
    }

    public async Task<ValidationResult> CreateOrUpdateValidation()
    {
        
    }
}