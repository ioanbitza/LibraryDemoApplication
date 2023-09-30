using LM.Domain.SeedWork;

namespace LM.Domain.Aggregates.Book
{
    public class Book : Entity, IAggregateRoot
    {
        private readonly List<BookItem> _bookItems = new();

        public Book(string title, Author author, ISBN isbn, Money rentPrice) : base()
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
            _bookItems.Add(new BookItem(rentPrice));
        }

        public virtual IReadOnlyList<BookItem> BookItems => _bookItems.AsReadOnly();
        public string Title { get; private set; }
        public Author Author { get; private set; }
        public ISBN ISBN { get; private set; }
        public int Availables => _bookItems.Count(x => x.IsAvailable);
        public int Totals => _bookItems.Count();

        public void AddItem(Money rentPrice)
        {
            var item = new BookItem(rentPrice);
            _bookItems.Add(item);
        }

        public object Where()
        {
            throw new NotImplementedException();
        }
    }
}
