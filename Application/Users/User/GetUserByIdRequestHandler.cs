using Application.Common.Model;
using Application.DTOs.Users;
using Domain.Common.Validation;
using Domain.Persistence.User;

namespace Application.Users.User;

public class GetUserByIdRequest
{
    public int Id { get; set; }
}

public class GetUserByIdRequestHandler : RequestHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    private readonly IUserUnitOfWork _unitOfWork;
    
    public GetUserByIdRequestHandler(IUserUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
    protected async override Task<Result<GetUserByIdResponse>> HandleRequest(GetUserByIdRequest request, Result<GetUserByIdResponse> result)
    {
        var user = await _unitOfWork.Repository.GetById(request.Id);

        if (user == null)
        {
            var validationResult = new ValidationResult();
            validationResult.AddValidationItems(new ValidationItem
            {
                Code = "{UsersEmpty}",
                Message = "Nema korisnika u bazi podataka",
                ValidationSeverity = ValidationSeverity.Error,
                ValidationType = ValidationType.BusinessRule
            });

            result.SetValidationResult(validationResult);
            return result;
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Username = user.Username,
            AdressStreet = user.AdressStreet,
            AdressCity = user.AdressCity,
            GeoLat = user.GeoLat,
            GeoLng = user.GeoLng,
            Website = user.Website,
            IsActive = user.IsActive
        };

        result.SetResult(new GetUserByIdResponse {User = userDto});
        return result;
    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }

}