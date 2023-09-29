using LM.Domain.ValueObjects;
using MediatR;

namespace LM.Domain.DomainEvents
{
    public class BookLoanedEvent : INotification
    {
        public ISBN ISBN { get; }

        public BookLoanedEvent(ISBN isbn)
        {
            ISBN = isbn;
        }
    }
}
