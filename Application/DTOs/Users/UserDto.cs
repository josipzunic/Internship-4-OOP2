namespace Application.DTOs.Users;

public class UserDto
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
    public bool IsActive { get; set; }
}