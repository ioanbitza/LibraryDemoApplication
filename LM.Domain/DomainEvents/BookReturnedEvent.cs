using LM.Domain.Enums;
using MediatR;

namespace LM.Domain.DomainEvents
{
    public class BookReturnedEvent : INotification
    {
        public Guid BookItemId { get; }
        public DateTime ReturnDate { get; }
        public BookQualityState QualityState { get; }
        public BookReturnedEvent(Guid bookItemId, BookQualityState qualityState, DateTime returnDate)
        {
            BookItemId = bookItemId;
            QualityState = qualityState;
            ReturnDate = returnDate;
        }
    }
}
