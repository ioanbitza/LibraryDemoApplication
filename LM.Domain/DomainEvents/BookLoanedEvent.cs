using MediatR;

namespace LM.Domain.DomainEvents
{
    public class BookLoanedEvent : INotification
    {
        public Guid BookItemId { get; }

        public BookLoanedEvent(Guid bookItemId)
        {
            BookItemId = bookItemId;
        }
    }
}
