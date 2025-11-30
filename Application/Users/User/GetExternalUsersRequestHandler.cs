using Application.Common.Model;
using Application.DTOs.Users;
using Domain.Common.Validation;
using Domain.Persistence.User;
using Infrastructure.Persistence;

namespace Application.Users.User;

public class GetExternalUsersRequest {}

public class GetExternalUsersRequestHandler : RequestHandler<GetExternalUsersRequest, GetExternalUsersResponse>
{
    private const string key = "externalUsers";
    private readonly IUserUnitOfWork _unitOfWork;
    private readonly IRetrieveExternalUsers _externalUserService;
    private readonly ICacheServices _cacheService;

    public GetExternalUsersRequestHandler(IUserUnitOfWork unitOfWork,  IRetrieveExternalUsers externalUserService, 
        ICacheServices cacheService)
    {
        _unitOfWork = unitOfWork;
        _externalUserService = externalUserService;
        _cacheService = cacheService;
    }

    protected override async Task<Result<GetExternalUsersResponse>> HandleRequest(GetExternalUsersRequest request,
        Result<GetExternalUsersResponse> result)
    {
        var usersFromCache = await _cacheService.Get<List<UserDto>>(key);
        if (usersFromCache != null)
        {
            result.SetResult(new GetExternalUsersResponse { Users = usersFromCache });
            return result;
        }

        var externalUsers = await _externalUserService.RetrieveExternalUsersAsync();

        var dtoList = externalUsers.Select(u => new UserDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Username = u.Username,
            AddressStreet = u.AddressStreet,
            AddressCity = u.AddressCity,
            GeoLat = u.GeoLat,
            GeoLng = u.GeoLng,
            Website = u.Website,
            IsActive = true
        }).ToList();
        
        var currentTime = DateTime.UtcNow;
        var endOfDay = currentTime.Date.AddDays(1);
        TimeSpan expiration = endOfDay.Subtract(currentTime); 
        
        await _cacheService.Set(key, dtoList, expiration);
        
        var validationResult = new ValidationResult();


        foreach (var dto in dtoList)
        {
            var existingUser = await _unitOfWork.Repository.GetByUsername(dto.Username);
            if (existingUser != null)
            {
                validationResult.AddValidationItems(new ValidationItem
                {
                    Code = "{UserAlreadyExists}",
                    Message = $"Korisnik '{dto.Username}' već postoji",
                    ValidationSeverity = ValidationSeverity.Error,
                    ValidationType = ValidationType.BusinessRule
                });
            }
        }


        if (validationResult.HasErrors)
        {
            result.SetValidationResult(validationResult);
            return result;
        }


        foreach (var dto in dtoList)
        {
            var newUser = new Domain.Entities.Users.User
            {
                Name = dto.Name,
                Email = dto.Email,
                Username = dto.Username,
                AddressStreet = dto.AddressStreet,
                AddressCity = dto.AddressCity,
                GeoLat = dto.GeoLat,
                GeoLng = dto.GeoLng,
                Website = dto.Website,
                Password = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
    
            await _unitOfWork.Repository.InsertAsync(newUser);
        }

        await _unitOfWork.SaveAsync();

        result.SetResult(new GetExternalUsersResponse { Users = dtoList });
        return result;

    }

    protected override Task<bool> IsAuthorized()
    {
        return Task.FromResult(true);
    }
}