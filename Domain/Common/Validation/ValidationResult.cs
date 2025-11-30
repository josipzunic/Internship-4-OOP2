using System.Collections.Generic;
namespace Domain.Common.Validation;
using System.Linq;

public class ValidationResult
{
    private List<ValidationItem> _validationItems;
    public IReadOnlyList<ValidationItem>  ValidationItems => _validationItems;
    
    public bool HasErrors => _validationItems.Any(validationResult => 
                             validationResult.ValidationSeverity == ValidationSeverity.Error);
    
    public void AddValidationItems(ValidationItem validationItem)
    {
        _validationItems.Add(validationItem);
    }
}