using Application.Common.Model;
using Application.DTOs.Users;
using Domain.Common.Validation;
using Domain.Persistence.Common;
using Domain.Persistence.User;

namespace Application.Users.User;

public class GetAllUsersRequest {}

public class GetAllUsersRequestHandler : RequestHandler<GetAllUsersRequest, GetAllUserResponse>
{
    private readonly IUserUnitOfWork _unitOfWork;

    public GetAllUsersRequestHandler(IUserUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    protected async override Task<Result<GetAllUserResponse>> HandleRequest(GetAllUsersRequest request, Result<GetAllUserResponse> result)
    {
        var users = await _unitOfWork.Repository.GetAllUsersAsync();

        if (users == null || !users.Any())
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

        var dtoList = users.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Username = u.Username,
            Password = u.Password,
            AddressStreet = u.AddressStreet,
            AddressCity = u.AddressCity,
            GeoLat = u.GeoLat,
            GeoLng = u.GeoLng,
            Website = u.Website,
            IsActive = u.IsActive
        }).ToList();

        result.SetResult(new GetAllUserResponse{Users = dtoList});
        return result;
    }

    protected override Task<bool> IsAuthorized() => Task.FromResult(true);
}
