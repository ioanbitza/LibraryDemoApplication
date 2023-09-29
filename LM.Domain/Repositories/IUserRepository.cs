using LM.Domain.Aggregates.Identity;

namespace LM.Domain.Repositories
{
    public interface IUserRepository
    {
        User GetUserByUsername(string username);
    }
}
