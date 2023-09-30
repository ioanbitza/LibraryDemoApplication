using LM.Domain.DomainEvents;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{ 
    public class BookReturnedEventHandler : INotificationHandler<BookReturnedEvent>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoanRepository _loanRepository;

        public BookReturnedEventHandler(IBookRepository bookRepository, ILoanRepository loanRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _loanRepository = loanRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public Task Handle(BookReturnedEvent notification, CancellationToken cancellationToken)
        {
            var book = _bookRepository.FindByBookItemId(notification.BookItemId);
            var loan = _loanRepository.FindByBookItemId(notification.BookItemId);
            var bookItem = book.BookItems.Single(item => item.Id == notification.BookItemId);
            var differenceQualityState = notification.QualityState.Id - bookItem.QualityState.Id;

            loan.CalculatePenalty(bookItem.RentPrice, differenceQualityState);

            bookItem.ReturnBook(notification.QualityState);

            _bookRepository.Update(book);

            return Task.CompletedTask;
        }
    }
}