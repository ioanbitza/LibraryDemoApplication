using LM.Domain.Aggregates.Book;
using LM.Domain.ValueObjects;

namespace LM.Domain.Repositories
{
    public interface IBookRepository
    {
        void AddBook(Book book);
        Book GetBookByISBN(ISBN isbn);
        IQueryable<Book> GetAllBooks();
        Book RemoveBook(ISBN isbn);
    }
}
