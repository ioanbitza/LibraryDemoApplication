using LM.Domain.DomainEvents;
using MediatR;

namespace LM.Infrastructure.EventHandlers
{
    public class BookLoanedEventHandler : INotificationHandler<BookLoanedEvent>
    {
        public Task Handle(BookLoanedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Book with ISBN {notification.ISBN} has been loaned.");
            return Task.CompletedTask;
        }
    }
}