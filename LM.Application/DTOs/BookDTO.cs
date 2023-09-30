using LM.Application.Utilities;
using LM.Domain.Aggregates.Book;

namespace LM.Application.DTOs
{
    public class BookDTO
    {
        public BookDTO(Book book)
        {
            Title = book.Title;
            Author = BookValueConverter.CreateStringFromAuthor(book.Author);
            ISBN = book.ISBN.Value.ToString();
            Totals = book.Totals;
            Availables = book.Totals;
            BookItems = book.BookItems.Select(bi => new BookItemDTO(bi)).ToList();
        }
        public string ISBN { get; }
        public string Title { get; }
        public string Author { get; }
        public int Totals { get; }
        public int Availables { get; }
        public List<BookItemDTO> BookItems { get; }
    }
}
