using LM.Application.Commands;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
    {
        private readonly ILoanRepository _loanRepository;

        public ReturnBookCommandHandler(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var loan = _loanRepository.FindByBookItemId(request.BookItemId);

            loan.ReturnBook(request.BookQuality, request.ReturnDate);

            _loanRepository.Update(loan);

            return Unit.Task;
        }
    }
}
