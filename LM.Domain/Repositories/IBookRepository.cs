using LM.Domain.Aggregates.Book;

namespace LM.Domain.Repositories
{
    public interface IBookRepository
    {
        void Add(Book book);
        void Update(Book book);
        Book FindByISBN(ISBN isbn);
        Book FindByBookItemId(Guid bookItemId);
        IQueryable<Book> GetAll();

        Book Remove(ISBN isbn);
    }
}
