using LM.Application.Commands;
using LM.Domain.Repositories;
using LM.Infrastructure.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class LoanBookCommandHandler : IRequestHandler<LoanBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public LoanBookCommandHandler(IBookRepository bookRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public Task Handle(LoanBookCommand request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetBookByISBN(request.ISBN);
            var user = _userRepository.GetUserByUsername(request.Username);
            book.LoanBook(request.LoanDate, user.Id);

            return Unit.Task;
        }
    }
}