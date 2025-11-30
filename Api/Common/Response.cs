using Application.Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

public class Response<TValue> where TValue : class
{
    public IReadOnlyList<ValidationResultItem> Errors { get; set; }
    
    public TValue? Value { get; set; }
    
    public Guid RequestId { get; set; }

    public Response(Result<TValue> result)
    {
        Errors = result.Errors;
        Value = result.Value;
        RequestId = result.RequestId;
    }
}