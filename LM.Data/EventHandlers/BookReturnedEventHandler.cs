using LM.Domain.DomainEvents;
using MediatR;

namespace LM.Infrastructure.EventHandlers
{
    public class BookReturnedEventHandler : INotificationHandler<BookReturnedEvent>
    {
        public Task Handle(BookReturnedEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Book with ISBN {notification.ISBN} has been returned.");
            return Task.CompletedTask;
        }
    }
}