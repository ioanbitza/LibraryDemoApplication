using LM.Domain.DomainEvents;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class BookLoanedEventHandler : INotificationHandler<BookLoanedEvent>
    {
        private readonly IBookRepository _bookRepository;

        public BookLoanedEventHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
        }

        public Task Handle(BookLoanedEvent notification, CancellationToken cancellationToken)
        {
            var book = _bookRepository.FindByBookItemId(notification.BookItemId);
            var bookItem = book.BookItems.Single(item => item.Id == notification.BookItemId);
            bookItem.LoanBook();
            _bookRepository.Update(book);
            return Task.CompletedTask;
        }
    }
}