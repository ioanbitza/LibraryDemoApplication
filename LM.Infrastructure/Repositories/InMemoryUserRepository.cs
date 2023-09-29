using LM.Domain.Aggregates.Book;
using LM.Domain.Aggregates.Identity;
using LM.Domain.DomainServices;
using LM.Domain.Enums;
using LM.Domain.Repositories;

namespace LM.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _users = new();
        private readonly IIdentityService _identityService;
        public InMemoryUserRepository(IIdentityService identityService)
        {
            _identityService = identityService;

            _users.Add(new User("Bibliotecar", identityService.HashPassword("ParolaBibliotecar"), UserRoleEnum.Librarian));
            _users.Add(new User("Cititor1", identityService.HashPassword("ParolaCititor1"), UserRoleEnum.Reader));
            _users.Add(new User("Cititor2", identityService.HashPassword("ParolaCititor2"), UserRoleEnum.Reader));
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));

            var user = _users.SingleOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? throw new KeyNotFoundException("User not found."); ;
            return user;
        }  
        
        public User AddUser(string username, string password, string role)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException(nameof(role));
            var isRole = Enum.TryParse<UserRoleEnum>(role, out var roleEnum);
            if (isRole) throw new ArgumentException($"{role} is not a valid Role.");

            User user = new(username, _identityService.HashPassword(password), roleEnum);

            _users.Add(user);

            return user;
        }
    }
}
