using LM.Application.Commands;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
    {
        private readonly IBookRepository _bookRepository;

        public ReturnBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.GetBookByISBN(request.ISBN);

            var loan = book.Loans.LastOrDefault(l => l.ReturnDate == null);
            if (loan == null)
                throw new InvalidOperationException("The loan not found.");

            loan.MarkAsReturned(request.ReturnDate);

            if (request.ReturnDate > loan.DueDate)
            {
                var penalty = loan.CalculatePenalty(book.RentPrice);
                Console.WriteLine($"Penalty due: {penalty} {book.RentPrice.Currency}");
            }

            book.ReturnBook(request.ReturnDate);

            return Unit.Task;
        }
    }
}
