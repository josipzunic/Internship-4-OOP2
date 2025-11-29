using Domain.Common.Validation;

namespace Application.Common.Model;

public class ValidationResultItem
{
    public ValidationSeverity ValidationSeverity { get; init; }
    public ValidationType ValidationType { get; init; }
    public string Message { get; init; }
    public string Code { get; init; }

    public static ValidationResultItem FormValidationItem(ValidationItem item)
    {
        return new ValidationResultItem
        {
            ValidationSeverity = item.ValidationSeverity,
            ValidationType = item.ValidationType,
            Message = item.Message,
            Code = item.Code
        };
    }
}