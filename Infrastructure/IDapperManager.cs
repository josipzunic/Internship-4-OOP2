namespace Infrastructure;

public interface IDapperManager
{
    Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null);
    Task<T?> QuerySingleAsync<T>(string sql, object? param = null);
    Task executeAsync(string sql, object? param = null);
}