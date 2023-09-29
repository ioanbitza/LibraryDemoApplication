using LM.Domain.DomainEvents;
using LM.Domain.ValueObjects;
using MediatR;

namespace LM.Domain.Aggregates.Book
{
    public class Book
    {
        private readonly List<Loan> _loans = new();
        private readonly List<INotification> _domainEvents;
        public Book(string title, string author, ISBN isbn, Money rentPrice)
        {
            _domainEvents = new List<INotification>();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            ISBN = isbn ?? throw new ArgumentNullException(nameof(isbn));
            RentPrice = rentPrice ?? throw new ArgumentNullException(nameof(rentPrice));
            IsAvailable = true;
        }
        public string Title { get; private set; }
        public string Author { get; private set; } // TODO: Create another entity named Author with First Name, Last Name
        public ISBN ISBN { get; private set; }
        public Money RentPrice { get; private set; }
        public bool IsAvailable { get; private set; }
        public virtual IReadOnlyList<Loan> Loans => _loans.AsReadOnly();
        public virtual IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public void LoanBook(DateTime loanDate, Guid userId)
        {
            if (!IsAvailable) throw new InvalidOperationException("Book is already loaned.");

            var loan = new Loan(this, userId, loanDate);
            _loans.Add(loan);
            IsAvailable = false;

            _domainEvents.Add(new BookLoanedEvent(ISBN));
        }

        public void ReturnBook(DateTime returnDate)
        {
            IsAvailable = true;
            // Find the loan and set its ReturnDate
            _domainEvents.Add(new BookReturnedEvent(ISBN));
        }
    }
}
