using LM.Domain.Enums;
using LM.Domain.SeedWork;

namespace LM.Domain.Aggregates.Book
{
    public class BookItem : Entity
    {
        public bool IsAvailable { get; private set; }
        public BookQualityState QualityState { get; private set; }
        public Money RentPrice { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public BookItem(Money rentPrice)
        {
            QualityState = BookQualityState.New;
            CreatedAt = DateTime.UtcNow;
            IsAvailable = true;
            RentPrice = rentPrice;
        }

        public void LoanBook()
        {
            IsAvailable = false;
        }

        public void ReturnBook(BookQualityState qualityState)
        {


            QualityState = qualityState;
            IsAvailable = true;
        }

        public void SetNewRentPrice(Money newRentPrice)
        {
            RentPrice = newRentPrice;
        }
    }
}
