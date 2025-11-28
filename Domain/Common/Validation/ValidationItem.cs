namespace Domain.Common.Validation;

public class ValidationItem
{
    public ValidationSeverity ValidationSeverity { get; init; }
    public ValidationType ValidationType { get; init; }
    public string Code { get; init; }
    public string Message { get; init; }
}