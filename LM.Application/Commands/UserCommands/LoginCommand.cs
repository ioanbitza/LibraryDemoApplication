using LM.Domain.Aggregates.Identity;
using MediatR;

namespace LM.Application.Commands
{
    public class LoginCommand : IRequest<User>
    {
        public string Username { get; }
        public string Password { get; }

        public LoginCommand(string username, string password)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }
    }
}
