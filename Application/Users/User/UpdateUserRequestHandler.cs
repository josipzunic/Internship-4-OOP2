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
    public string Password { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressCity { get; set; }
    public double? GeoLat { get; set; }
    public double? GeoLng { get; set; }
    public string? Website { get; set; }
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

        var passwordValidity = await _unitOfWork.Repository.GetById(user.Id);
        if (passwordValidity == null || passwordValidity.Password == ""  || passwordValidity.Password == null)
            validatioResult.AddValidationItems(ValidationItems.User.PasswordRequired);

        var activeUsers = await _unitOfWork.Repository.GetAllActiveUsersAsync();
        foreach (var activeUser in activeUsers.Where(u => u.Id != user.Id))
        {
            double distance = DistanceHelper.Distance(user.GeoLat ?? 0, user.GeoLng ?? 0, activeUser.GeoLat ?? 0, activeUser.GeoLng ?? 0);
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
        user.Password = request.Password;
        user.AddressStreet = request.AddressStreet;
        user.AddressCity = request.AddressCity;
        user.GeoLat = request.GeoLat;
        user.GeoLng = request.GeoLng;
        user.Website = request.Website;
        user.UpdatedAt = DateTime.UtcNow;
        if (user.CreatedAt.Kind == DateTimeKind.Unspecified)
            user.CreatedAt = DateTime.SpecifyKind(user.CreatedAt, DateTimeKind.Utc);
    }
}