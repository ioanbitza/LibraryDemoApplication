using LM.Domain.Aggregates.Book;
using LM.Domain.Enums;
using LM.Domain.Repositories;
using LM.Domain.ValueObjects;
using System.Collections.Concurrent;
using System.Linq;

namespace LM.Infrastructure.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly ConcurrentDictionary<ISBN, Book> _books = new();
        public InMemoryBookRepository()
        {
            var book1 = new Book("Atomic Habits", "James Clear", new ISBN("9780735211292"), new Money(70m, CurrencyEnum.RON));
            _books.TryAdd(new ISBN("9780735211292"), book1);

            var book2 = new Book("The Pragmatic Programmer", "Andrew Hunt", new ISBN("9780201616224"), new Money(50m, CurrencyEnum.RON));
            _books.TryAdd(new ISBN("9780201616224"), book2);

            var book3 = new Book("Clean Code", "Robert C. Martin", new ISBN("9780132350884"), new Money(80m, CurrencyEnum.RON));
            _books.TryAdd(new ISBN("9780132350884"), book3);

            var book4 = new Book("Sapiens: A Brief History of Humankind", "Yuval Noah Harari", new ISBN("9780062316110"), new Money(75m, CurrencyEnum.RON));
            _books.TryAdd(new ISBN("9780062316110"), book4);

            var book5 = new Book("Educated", "Tara Westover", new ISBN("9780399590504"), new Money(44m, CurrencyEnum.RON));
            _books.TryAdd(new ISBN("9780399590504"), book5);

            var book6 = new Book("Educated", "Tara Westover", new ISBN("9780399590744"), new Money(44m, CurrencyEnum.RON));
            _books.TryAdd(new ISBN("9780399590744"), book6);
        }

        public void AddBook(Book book)
        {
            _books.TryAdd(book.ISBN, book);
        }

        public Book GetBookByISBN(ISBN isbn)
        {
            bool exists = _books.TryGetValue(isbn, out var book);
            if (!exists || book is null) throw new KeyNotFoundException("Book not found.");
            return book;
        }

        public IQueryable<Book> GetAllBooks()
        {
            return _books.Values.AsQueryable();
        }

        public Book RemoveBook(ISBN isbn)
        {
            _books.TryRemove(isbn, out var book);
            if (book is null) throw new KeyNotFoundException("Book not found.");
            return book;
        }
    }
}