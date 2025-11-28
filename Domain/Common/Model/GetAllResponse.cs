namespace Domain.Common.Model;

public class GetAllResponse<TValue>
{
    public IEnumerable<TValue> Values { get; init; }
}