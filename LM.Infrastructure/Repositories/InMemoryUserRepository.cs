using LM.Domain.Aggregates.Identity;
using LM.Domain.Enums;
using LM.Domain.Repositories;
using LM.Domain.SeedWork;
using LM.Infrastructure.Utilities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace LM.Infrastructure.Repositories
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = new();

        public InMemoryUserRepository()
        {
            var librarian = new User("Bibliotecar", PasswordHelper.HashPassword("ParolaBibliotecar"), UserRole.Librarian);
            _users.TryAdd(librarian.Id, librarian);
            var reader1 = new User("Cititor1", PasswordHelper.HashPassword("ParolaCititor1"), UserRole.Reader);
            _users.TryAdd(reader1.Id, reader1);
            var reader2 = new User("Cititor2", PasswordHelper.HashPassword("ParolaCititor2"), UserRole.Reader);
            _users.TryAdd(reader2.Id, reader2);
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));

            var user = _users.Values.SingleOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ?? throw new KeyNotFoundException("User not found."); ;
            return user;
        }  
        
        //TO DO
        public User AddUser(string username, string password, string role)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrEmpty(role)) throw new ArgumentNullException(nameof(role));
            var roleEnum = Enumeration.FromDisplayName<UserRole>(role);
            if (roleEnum is null) throw new ArgumentException($"{role} is not a valid Role.");

            var user = new User(username, PasswordHelper.HashPassword(password), roleEnum);

            _users.TryAdd(user.Id, user);

            return user;
        }
    }
}
