using Application.Common.Model;
using Domain.Common.Validation;
using Domain.Common.Validation.ValidationItems;
using Domain.Persistence.User;
using Infrastructure.Common;

namespace Application.Users.User;

public class UpdateUserRequest
{
    public int Id { get; set; } 
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string AdressStreet { get; set; }
    public string AdressCity { get; set; }
    public decimal GeoLat { get; set; }
    public decimal GeoLng { get; set; }
    public string Website { get; set; }
}

public class UpdateUserRequestHandler : RequestHandler<UpdateUserRequest, SuccessPostResponse>
{
    private readonly IUserUnitOfWork _unitOfWork;

    public UpdateUserRequestHandler(IUserUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    protected override async Task<Result<SuccessPostResponse>> HandleRequest(UpdateUserRequest request, Result<SuccessPostResponse> result)
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
        
        UpdateUser(user, request);

        var validatioResult = await user.CreateOrUpdateValidation();
        
        var existingEmail = await _unitOfWork.Repository.EmailExistsAsync(user.Email);
        if (existingEmail != null && existingEmail.Id != user.Id)
            validatioResult.AddValidationItems(ValidationItems.User.EmailAlreadyExists);
        
        var existingUsername = await _unitOfWork.Repository.UsernameExistsAsync(user.Username);
        if (existingUsername != null && existingUsername.Id != user.Id)
            validatioResult.AddValidationItems(ValidationItems.User.UsernameAlreadyExists);

        var activeUsers = await _unitOfWork.Repository.GetAllActiveUsersAsync();
        foreach (var activeUser in activeUsers.Where(u => u.Id != user.Id))
        {
            decimal distance = DistanceHelper.Distance(user.GeoLat, user.GeoLng, activeUser.GeoLat, activeUser.GeoLng);
            if (distance < 3)
            {
                validatioResult.AddValidationItems(ValidationItems.User.Within3KmExists);
                break;
            }
        }

        result.SetValidationResult(validatioResult);
        if (validatioResult.HasErrors)
            return result;
        
        _unitOfWork.Repository.Update(user);
        await _unitOfWork.SaveAsync();
        
        result.SetResult(new SuccessPostResponse(user.Id));
        return result;

    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }

    private void UpdateUser(Domain.Entities.Users.User user, UpdateUserRequest request)
    {
        user.Name = request.Name;
        user.Username = request.Username;
        user.Email = request.Email;
        user.AdressStreet = request.AdressStreet;
        user.AdressCity = request.AdressCity;
        user.GeoLat = request.GeoLat;
        user.GeoLng = request.GeoLng;
        user.Website = request.Website;
        user.UpdatedAt = DateTime.UtcNow;
    }
}