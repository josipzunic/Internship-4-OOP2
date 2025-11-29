using Domain.Common.Validation;

namespace Application.Common.Model;

public class Result<TValue> where TValue : class
{
    private List<ValidationResultItem> _errors = new ();
    
    public TValue? Value { get; set; }
    public Guid RequestId { get; set; }
    public bool IsAuthorized { get; set; } = true;

    public IReadOnlyList<ValidationResultItem> Errors
    {
        get => _errors;
        init => _errors.AddRange(value);
    }
    
    public bool HasErrors => Errors.Any(validationResult => validationResult.ValidationSeverity == ValidationSeverity.Error);

    public void SetResult(TValue value)
    {
        Value = value;
    }
    
    public void SetValidationResult(ValidationResult validationResult)
    {
        _errors.AddRange(validationResult.ValidationItems.
            Where(x => x.ValidationSeverity == ValidationSeverity.Error)
            .Select(x => ValidationResultItem.FormValidationItem(x)));
    }
    
    public void SetUnauthorizedResult()
    {
        Value = null;
        IsAuthorized = false;
    }
}