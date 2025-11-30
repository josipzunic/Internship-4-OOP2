using Application.Common.Model;
using Domain.Common.Validation;
using Domain.Persistence.User;

namespace Application.Users.User;

public class ActivateUserRequest
{
    public int Id { get; set; }
    
}

public class ActivateUserRequestHandler : RequestHandler<ActivateUserRequest, SuccessPostResponse>
{
    private readonly IUserUnitOfWork _unitOfWork;

    public ActivateUserRequestHandler(IUserUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    protected async override Task<Result<SuccessPostResponse>> HandleRequest(ActivateUserRequest request, Result<SuccessPostResponse> result)
    {
        var user = await _unitOfWork.Repository.GetById(request.Id);
        if (user == null)
        {
            var validationResult = new ValidationResult();
            validationResult.AddValidationItems(new ValidationItem
            {
                Code = "{UserNotFound}",
                Message = "Korisnik s danim id-om nije pronađen",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BusinessRule
            });
            
            result.SetValidationResult(validationResult);
            return result;
        }
        
        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        
        _unitOfWork.Repository.Update(user);
        await _unitOfWork.SaveAsync();
        
        result.SetResult(new SuccessPostResponse(user.Id));
        return result;
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}