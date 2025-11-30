namespace Infrastructure.Persistence;

public interface ICacheServices
{
    Task<TValue> Get<TValue>(string key);
    Task Set<TValue>(string key, TValue value, TimeSpan expiration);
}