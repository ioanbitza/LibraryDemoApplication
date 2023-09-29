using LM.Domain.Aggregates.Identity;
using LM.Domain.ValueObjects;

namespace LM.Domain.Aggregates.Book
{
    public class Loan
    {
        public Guid Id { get; private set; }
        public Book Book { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime LoanDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }

        public Loan(Book book, Guid userId)
        {
            Book = book;
            UserId = userId;
            LoanDate = DateTime.Now;
            DueDate = LoanDate.AddDays(14); // 2 weeks
            ReturnDate = null;
        }

        public Loan(Book book, Guid userId, DateTime dueDate)
        {
            Book = book;
            UserId = userId;
            LoanDate = DateTime.Now;
            DueDate = dueDate;
            ReturnDate = null;
        }

        public decimal CalculatePenalty(Money rentPrice)
        {
            var overdueDays = (DateTime.Now - DueDate).Days;
            if (overdueDays <= 0) return 0;
            return overdueDays * (rentPrice.Amount * 0.01m); // 1% of rent price per day
        }

        public void MarkAsReturned(DateTime returnDate)
        {
            ReturnDate = returnDate;
        }
    }
}
