using LM.Domain.Aggregates.Book;
using LM.Domain.Enums;
using LM.Domain.Repositories;
using LM.Domain.SeedWork;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;

namespace LM.Infrastructure.Repositories
{
    public class InMemoryBookRepository : IBookRepository
    {
        private readonly ConcurrentDictionary<ISBN, Book> _books = new();
        public InMemoryBookRepository()
        {
            var book1 = new Book("Atomic Habits", new Author("James", "Clear"), new ISBN("9780735211292"), new Money(70m, Currency.RON));
            _books.TryAdd(new ISBN("9780735211292"), book1);

            var book2 = new Book("The Pragmatic Programmer", new Author("Andrew", "Hunt"), new ISBN("9780201616224"), new Money(50m, Currency.RON));
            _books.TryAdd(new ISBN("9780201616224"), book2);

            var book3 = new Book("Clean Code", new Author("Robert", "Martin" , "Cecil"), new ISBN("9780132350884"), new Money(80m, Currency.RON));
            _books.TryAdd(new ISBN("9780132350884"), book3);

            var book4 = new Book("Sapiens: A Brief History of Humankind", new Author("Yuval Noah", "Harari"), new ISBN("9780062316110"), new Money(75m, Currency.RON));
            _books.TryAdd(new ISBN("9780062316110"), book4);

            var book5 = new Book("Educated", new Author("Tara", "Westover"), new ISBN("9780399590504"), new Money(44m, Currency.RON));
            _books.TryAdd(new ISBN("9780399590504"), book5);
        }

        public void Add(Book book)
        {
            _books.TryAdd(book.ISBN, book);
        }

        public void Update(Book updatedBook)
        {
            if (updatedBook == null) throw new ArgumentNullException(nameof(updatedBook));

            if (_books.TryGetValue(updatedBook.ISBN, out var existingBook))
            {
                _books.TryUpdate(updatedBook.ISBN, updatedBook, existingBook);
            }
            else
            {
                throw new KeyNotFoundException($"No book found with ISBN {updatedBook.ISBN}.");
            }
        }

        public Book FindByISBN(ISBN isbn)
        {
            bool exists = _books.TryGetValue(isbn, out var book);
            if (!exists || book is null) throw new KeyNotFoundException("Book not found.");
            return book;
        }

        public Book FindByBookItemId(Guid bookItemId)
        {
            var book = _books.Values.SingleOrDefault(b => b.BookItems.Any(bookItem => bookItem.Id == bookItemId));

            return book is null ? throw new KeyNotFoundException("Book not found.") : book;
        }

        public IQueryable<Book> GetAll()
        {
            return _books.Values.AsQueryable();
        }

        public Book Remove(ISBN isbn)
        {
            _books.TryRemove(isbn, out var book);
            if (book is null) throw new KeyNotFoundException("Book not found.");
            return book;
        }

    
    }
}