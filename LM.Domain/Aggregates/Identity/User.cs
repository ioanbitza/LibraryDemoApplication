using LM.Domain.Enums;
using LM.Domain.SeedWork;

namespace LM.Domain.Aggregates.Identity
{
    public class User : Entity
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Role { get; private set; }

        public User(string username, string passwordHash, UserRole role) : base()
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Role = role;
        }
    }
}
