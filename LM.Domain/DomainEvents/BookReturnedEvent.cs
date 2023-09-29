using LM.Domain.ValueObjects;
using MediatR;

namespace LM.Domain.DomainEvents
{
    public class BookReturnedEvent : INotification
    {
        public ISBN ISBN { get; }

        public BookReturnedEvent(ISBN isbn)
        {
            ISBN = isbn;
        }
    }
}
