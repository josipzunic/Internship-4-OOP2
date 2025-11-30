using System.Threading.Tasks;
using Domain.Common.Model;
using Domain.Common.Validation;
using Domain.Common.Validation.ValidationItems;
using Domain.Persistence.User;
using System;
namespace Domain.Entities.Users;

public class User
{
    public const int MaxNameLength = 100;
    public const int MaxAdressLength = 150;
    public const int MaxAdressCityLength = 100;
    public const int MaxWebsiteLength = 100;
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string AdressStreet { get; set; }
    public string AdressCity { get; set; }
    public decimal GeoLat { get; set; }
    public decimal GeoLng { get; set; }
    public string? Website { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;

    public async Task<Result<bool>> Create(IUserRepository userRepository)
    {
        ValidationResult validationResult = await CreateOrUpdateValidation();
        if (validationResult.HasErrors)
            return new Result<bool>(false, validationResult);

        await userRepository.InsertAsync(this);
        
        return new Result<bool>(true, validationResult);
    }
    public async Task<ValidationResult> CreateOrUpdateValidation()
    {
        var validationResult = new ValidationResult();
        
        if (string.IsNullOrWhiteSpace(Name))
            validationResult.AddValidationItems(ValidationItems.User.NameRequired);
        else 
            if(Name.Length > MaxNameLength) 
                validationResult.AddValidationItems(ValidationItems.User.MaxNameLength);
        
        if (string.IsNullOrWhiteSpace(AdressStreet))
            validationResult.AddValidationItems(ValidationItems.User.AdressStreetRequired);
        else 
            if (AdressStreet.Length > MaxAdressLength)
                validationResult.AddValidationItems(ValidationItems.User.MaxAdressLength);
        
        if(string.IsNullOrWhiteSpace(AdressCity))
            validationResult.AddValidationItems(ValidationItems.User.AdressCityRequired);
        else 
            if (AdressCity.Length > MaxAdressCityLength)
                validationResult.AddValidationItems(ValidationItems.User.MaxAdressCityLength);

        if (!string.IsNullOrWhiteSpace(Website))
        {
            if (!IsValidUrl(Website))
                validationResult.AddValidationItems(ValidationItems.User.IncorrectUrlFormat);
            else if(Website.Length > MaxWebsiteLength)
                validationResult.AddValidationItems(ValidationItems.User.MaxWebsiteLength);
        }
        
        if (string.IsNullOrWhiteSpace(Email))
            validationResult.AddValidationItems((ValidationItems.User.EmailRequired));
        else 
            if (!IsValidEmail(Email))
                validationResult.AddValidationItems(ValidationItems.User.IncorrectEmailFormat);

        if (GeoLat > 90m || GeoLat < -90m)
            validationResult.AddValidationItems(ValidationItems.User.GeoLatRange);
        
        if (GeoLng > 180m || GeoLng < -180m)
            validationResult.AddValidationItems(ValidationItems.User.GeoLngRange);
        
        if (string.IsNullOrWhiteSpace(Username))
            validationResult.AddValidationItems(ValidationItems.User.UsernameRequired);
        
        
        return validationResult;
    }
    
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;

    public bool IsValidUrl(string url)
    {
        bool isValid = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                       && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        return isValid;
    }

    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    
}