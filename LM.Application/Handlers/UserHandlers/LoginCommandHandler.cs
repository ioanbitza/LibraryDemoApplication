using LM.Application.Commands;
using LM.Application.Queries;
using LM.Application.Utilities;
using LM.Domain.Aggregates.Identity;
using MediatR;

namespace LM.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, User>
    {
        private readonly IMediator _mediator;

        public LoginCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<User?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(request.Username), cancellationToken);
            if (user is not null && PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

    }
}
