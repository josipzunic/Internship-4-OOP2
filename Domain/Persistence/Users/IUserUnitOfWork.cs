using Domain.Persistence.Common;

namespace Domain.Persistence.User;

public interface IUserUnitOfWork :  IUnitOfWork
{
    IUserRepository Repository { get; }
}