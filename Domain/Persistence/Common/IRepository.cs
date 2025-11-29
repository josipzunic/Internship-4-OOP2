using System.Threading.Tasks;
using Domain.Common.Model;

namespace Domain.Persistence.Common;

public interface IRepository<TEntity, TId> where TEntity : class
{
    Task<GetAllResponse<TEntity>> Get();
    Task InsertAsync(TEntity entity);
    void Update(TEntity entity);
    Task DeleteAsync(TId id);
    void Delete(TEntity? entity);
}