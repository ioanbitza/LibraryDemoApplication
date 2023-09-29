using LM.Domain.Aggregates.Identity;
using MediatR;

namespace LM.Application.Queries
{
    public class GetUserByUsernameQuery : IRequest<User>
    {
        public string Username { get; }

        public GetUserByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
