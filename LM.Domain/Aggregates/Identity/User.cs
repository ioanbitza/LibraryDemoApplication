using LM.Domain.Enums;

namespace LM.Domain.Aggregates.Identity
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRoleEnum Role { get; private set; }

        public User(string username, string passwordHash, UserRoleEnum role)
        {
            Id  = Guid.NewGuid();
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            Role = role;
        }
    }

}
