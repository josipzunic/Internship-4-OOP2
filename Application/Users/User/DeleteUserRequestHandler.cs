using Application.Common.Model;
using Domain.Common.Validation;
using Domain.Persistence.User;

namespace Application.Users.User;

public class DeleteUserRequest
{
    public int Id { get; set; }
}

public class DeleteUserRequestHandler : RequestHandler<DeleteUserRequest, SuccessPostResponse>
{
    private readonly IUserUnitOfWork _unitOfWork;

    protected async override Task<Result<SuccessPostResponse>> HandleRequest(DeleteUserRequest request, Result<SuccessPostResponse> result)
    {
        var user = await _unitOfWork.Repository.GetById(request.Id);
        if (user == null)
        {
            var validationResult = new ValidationResult();
            validationResult.AddValidationItems(new ValidationItem
            {
                Code = "{UserNotFound}",
                Message = "User with given id was not found",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BusinessRule
            });
            
            result.SetValidationResult(validationResult);
            return result;
        }
        
        await _unitOfWork.Repository.DeleteAsync(user.Id);
        await _unitOfWork.SaveAsync();
        
        result.SetResult(new SuccessPostResponse(user.Id));
        return result;
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}