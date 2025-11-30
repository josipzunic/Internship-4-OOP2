namespace Domain.Common.Validation.ValidationItems;

public static partial class ValidationItems
{
    public static class User
    {
        public static string CodePrefix = nameof(User);

        public static readonly ValidationItem MaxNameLength = new ()
        {
            Code = $"{CodePrefix}1",
            Message = $"Ime ne smije biti duže od {Entities.Users.User.MaxNameLength} znakova",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };

        public static readonly ValidationItem MaxAdressLength = new ()
        {
            Code = $"{CodePrefix}2",
            Message = $"Adresa stanovanja ne smije biti duža od  {Entities.Users.User.MaxAdressLength} znakova",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };

        public static readonly ValidationItem MaxAdressCityLength = new()
        {
            Code = $"{CodePrefix}3",
            Message = $"Grad ne smije biti duži od {Entities.Users.User.MaxAdressCityLength} znakova",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };
        
        public static readonly ValidationItem MaxWebsiteLength = new()
        {
            Code = $"{CodePrefix}4",
            Message = $"URL ne smije biti duži od {Entities.Users.User.MaxWebsiteLength} znakova",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };
        
        public static readonly ValidationItem IncorrectUrlFormat = new()
        {
            Code = $"{CodePrefix}5",
            Message = "Dani URL nije dobroga formata",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };
        
        public static readonly ValidationItem NameRequired = new()
        {
            Code = $"{CodePrefix}6",
            Message = "Polje ime ne smije biti prazno",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };

        public static readonly ValidationItem IncorrectEmailFormat = new()
        {
            Code = $"{CodePrefix}9",
            Message = "Dana email adresa nije dobroga formata",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };

        public static readonly ValidationItem EmailRequired = new()
        {
            Code = $"{CodePrefix}10",
            Message = "Polje email ne smije biti prazno",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };

        public static readonly ValidationItem GeoLatRange = new()
        {
            Code = $"{CodePrefix}11",
            Message = "Geografska širina mora biti unutar intervala [-90, 90] stupnjeva",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };
        
        public static readonly ValidationItem GeoLngRange = new()
        {
            Code = $"{CodePrefix}12",
            Message = "Geografska dužina mora biti unutar intervala [-180, 180] stupnjeva",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };
        
        public static readonly ValidationItem UsernameRequired = new()
        {
            Code = $"{CodePrefix}13",
            Message = "Polje username ne smije biti prazno",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };

        public static readonly ValidationItem UsernameAlreadyExists = new()
        {
            Code = $"{CodePrefix}14",
            Message = "Korisnik s danim korisničkim imenom već postoji",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.BusinessRule
        };

        public static readonly ValidationItem EmailAlreadyExists = new()
        {
            Code = $"{CodePrefix}15",
            Message = "Korisnik s danom email adresom već postoji",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.BusinessRule
        };
        
        public static readonly ValidationItem Within3KmExists = new() 
        {
            Code = $"{CodePrefix}16",
            Message = "Postoji aktivni korisnik u radijusu od 3 km",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.BusinessRule
        };
        
        public static readonly ValidationItem PasswordRequired = new() 
        {
            Code = $"{CodePrefix}17",
            Message = "Polje lozinka ne smije biti prazno",
            ValidationSeverity = ValidationSeverity.Error,
            ValidationType = ValidationType.FormalValidation
        };
    }
}