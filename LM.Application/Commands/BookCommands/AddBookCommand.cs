using LM.Application.Utilities;
using LM.Domain.Aggregates.Book;
using LM.Domain.Enums;
using LM.Domain.SeedWork;
using MediatR;

namespace LM.Application.Commands
{
    public class AddBookCommand : IRequest
    {
        public string Title { get; }
        public Author Author { get; }
        public ISBN ISBN { get; }
        public Money RentPrice { get; }

        public AddBookCommand(string title, string author, string isbn, string rentPrice, string currency)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));

            Author = BookValueConverter.CreateAuthorFromString(author);

            ISBN = new ISBN(isbn) ?? throw new ArgumentNullException(nameof(isbn));

            if(!decimal.TryParse(rentPrice, out var price)) throw new ArgumentException("Rent price is not a number.");
            var currencyEnum = Enumeration.FromDisplayName<Currency>(currency);
            if (currencyEnum is null) throw new ArgumentException("The currency is invalid."); ;
            RentPrice = new Money(price, currencyEnum);
        }
    }
}
