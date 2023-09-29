using LM.Application.Queries;
using LM.Domain.Aggregates.Identity;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, User>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByUsernameQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<User> Handle(GetUserByUsernameQuery request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            return _userRepository.GetUserByUsername(request.Username);
        }
    }
}
