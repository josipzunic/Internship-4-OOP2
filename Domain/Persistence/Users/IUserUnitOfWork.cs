namespace Domain.Persistence.User;

public interface IUserUnitOfWork
{
    IUserRepository Repository { get; }
}