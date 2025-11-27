namespace Domain.Entities.Users;

public class User
{
    public const int MaxNameLength = 100;
    public const int MaxAdressStringLength = 150;
    public const int MaxAdressCityLength = 100;
    public const int MaxWebsiteLength = 200;
    
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
    public bool IsActive { get; set; }

    public async Task<Result<int>> Create()
    {
        
    }

    public async Task<ValidationResult> CreateOrUpdateValidation()
    {
        
    }
    
}