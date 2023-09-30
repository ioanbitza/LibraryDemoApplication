using LM.Application.Commands;
using LM.Domain.Aggregates.Loan;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class LoanBookCommandHandler : IRequestHandler<LoanBookCommand>
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userRepository;

        public LoanBookCommandHandler(ILoanRepository loanRepository, IBookRepository bookRepository, IUserRepository userRepository)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
        }

        public Task Handle(LoanBookCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetUserByUsername(request.Username);

            var book = _bookRepository.FindByISBN(request.ISBN);

            // We take by default the best book to rent,
            var bestQualityBookItem = book.BookItems
                              .Where(bi => bi.IsAvailable) // Only available books
                              .OrderByDescending(bi => bi.QualityState.Id) 
                              .FirstOrDefault() ?? throw new ArgumentException("Not found an available book.");
            var loan = new Loan(bestQualityBookItem.Id, user.Id, request.LoanDate);

            _loanRepository.Add(loan);

            return Unit.Task;
        }
    }
}