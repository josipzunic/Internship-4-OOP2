namespace Domain.Common.Model;
using System.Collections.Generic;

public class GetAllResponse<TValue>
{
    public IEnumerable<TValue> Values { get; init; }
}