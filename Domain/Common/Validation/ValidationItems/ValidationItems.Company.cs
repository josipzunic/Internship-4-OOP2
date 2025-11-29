namespace Domain.Common.Validation.ValidationItems;

public static partial class ValidationItems
{
    public static class Company
    {
        public static string CodePrefix = nameof(Company);

        public static readonly ValidationItem MaxCompanyNameLength = new()
        {
            Code = $"{CodePrefix}1",
            Message =
                $"Polje s imenom kompanije ne smije imati više od {Entities.Companies.Company.MaxCompanyNameLength}",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };

        public static readonly ValidationItem CompanyNameRequired = new()
        {
            Code = $"{CodePrefix}2",
            Message = "Polje s imenom kompanije ne smije biti prazno",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };
        
        public static readonly ValidationItem CompanyNameUnique = new()
        {
            Code = $"{CodePrefix}3",
            Message = "Kompanija s tim imenom već postoji",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.BusinessRule
        };
    }
}