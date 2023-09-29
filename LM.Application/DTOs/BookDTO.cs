using LM.Domain.Aggregates.Book;

namespace LM.ConsoleApplication.DTOs
{
    public class BookDTO
    {
        public string ISBN { get; }
        public string Title { get; }
        public string Author { get; }
        public bool IsAvailable { get; }
        public string RentPrice { get; }
        public List<LoanDTO> Loans { get; }

        public BookDTO(Book book)
        {
            Title = book.Title;
            Author = book.Author;
            ISBN = book.ISBN.Value.ToString();
            IsAvailable = book.IsAvailable;
            RentPrice = $"{book.RentPrice.Amount} {book.RentPrice.Currency}";
            Loans = book.Loans.Select(loan => new LoanDTO(loan)).ToList();
        }
    }
}
