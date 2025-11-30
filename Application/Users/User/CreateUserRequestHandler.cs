using Application.Common.Model;
using Domain.Common.Validation.ValidationItems;
using Domain.Persistence.User;
using Infrastructure.Common;

namespace Application.Users.User;


public class CreateUserRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressCity { get; set; }
    public double? GeoLat { get; set; }
    public double? GeoLng { get; set; }
    public string? Website { get; set; }
}
public class CreateUserRequestHandler : RequestHandler<CreateUserRequest, SuccessPostResponse>
{
    private readonly IUserUnitOfWork _unitOfWork;

    public CreateUserRequestHandler(IUserUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    protected async override Task<Result<SuccessPostResponse>> HandleRequest(CreateUserRequest request, Result<SuccessPostResponse> result)
    {
        var user = new Domain.Entities.Users.User
        {
            Name = request.Name,
            Email = request.Email,
            AddressStreet = request.AddressStreet,
            AddressCity = request.AddressCity,
            GeoLat = request.GeoLat,
            GeoLng = request.GeoLng,
            Website = request.Website,
            Username = request.Username,
            Password = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var validationResult = await user.Create(_unitOfWork.Repository);
        
        var existingEmail = await _unitOfWork.Repository.EmailExistsAsync(user.Email);
        if (existingEmail != null && existingEmail.Id != user.Id)
            validationResult.ValidationResult.AddValidationItems(ValidationItems.User.EmailAlreadyExists);
        
        var existingUsername = await _unitOfWork.Repository.UsernameExistsAsync(user.Username);
        if (existingUsername != null && existingUsername.Id != user.Id)
            validationResult.ValidationResult.AddValidationItems(ValidationItems.User.UsernameAlreadyExists);

        var activeUsers = await _unitOfWork.Repository.GetAllActiveUsersAsync();
        foreach (var activeUser in activeUsers.Where(u => u.Id != user.Id))
        {
            double distance = DistanceHelper.Distance(user.GeoLat ?? 0, user.GeoLng ?? 0, activeUser.GeoLat ?? 0, activeUser.GeoLng ?? 0);
            if (distance < 3)
            {
                validationResult.ValidationResult.AddValidationItems(ValidationItems.User.Within3KmExists);
                break;
            }
        }
        result.SetValidationResult(validationResult.ValidationResult);

        if (result.HasErrors)
            return result;

        await _unitOfWork.Repository.InsertAsync(user);
        await _unitOfWork.SaveAsync();
        
        result.SetResult(new SuccessPostResponse(user.Id));
        return result;
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}