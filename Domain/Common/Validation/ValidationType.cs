using System.Text.Json.Serialization;

namespace Domain.Common.Validation;


[JsonConverter(typeof(JsonStringEnumConverter))]

public enum ValidationType
{
    FormalValidation,
    BusinessRule,
    SystemError
}