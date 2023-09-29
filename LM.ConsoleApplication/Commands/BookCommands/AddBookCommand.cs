using LM.Domain.Enums;
using LM.Domain.ValueObjects;
using MediatR;

namespace LM.Application.Commands
{
    public class AddBookCommand : IRequest
    {
        public string Title { get; }
        public string Author { get; }
        public ISBN ISBN { get; }
        public Money RentPrice { get; }

        public AddBookCommand(string title, string author, string isbn, string rentPrice, string currency)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            ISBN = new ISBN(isbn) ?? throw new ArgumentNullException(nameof(isbn));
            if(!decimal.TryParse(rentPrice, out var price)) throw new ArgumentException("Rent price is not a number.");
            if(!Enum.TryParse<CurrencyEnum>(currency, out var currencyEnum)) throw new ArgumentException("The currency is invalid."); ;
            RentPrice = new Money(price, currencyEnum);
        }
    }
}
