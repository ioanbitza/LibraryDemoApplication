using LM.Application.Commands;
using LM.Application.Queries;
using LM.Domain.Aggregates.Identity;
using LM.Domain.DomainServices;
using MediatR;

namespace LM.Application.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, User>
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IMediator mediator, IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
        }

        public async Task<User?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserByUsernameQuery(request.Username), cancellationToken);
            if (user is not null && _identityService.VerifyPassword(request.Password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }
    }
}
