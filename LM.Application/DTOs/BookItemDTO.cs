using LM.Domain.Aggregates.Book;

namespace LM.Application.DTOs
{
    public class BookItemDTO
    {
        public BookItemDTO(BookItem bookItem)
        {
            ID = bookItem.Id.ToString();
            IsAvailable = bookItem.IsAvailable.ToString();
            QualityState = bookItem.QualityState.Name ?? throw new ArgumentNullException(nameof(bookItem.QualityState.Name));
            RentPrice = bookItem.RentPrice.GetFormattedMoney() ?? throw new ArgumentNullException(nameof(bookItem.RentPrice));
            DateRegistered = bookItem.CreatedAt.ToLongDateString();
        }

        public string ID { get; }
        public string IsAvailable { get; }
        public string QualityState { get; }
        public string RentPrice { get; }
        public string DateRegistered { get; }
    }
}
