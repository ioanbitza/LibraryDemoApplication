using LM.Application.Utilities;
using LM.Domain.Aggregates.Book;
using MediatR;

namespace LM.Application.Commands
{
    public class AddBookCommand : IRequest
    {
        public string Title { get; }
        public Author Author { get; }
        public ISBN ISBN { get; }
        public Money RentPrice { get; }

        public AddBookCommand(string title, string author, string isbn, string rentPrice)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));

            Author = BookValueConverter.CreateAuthorFromString(author);

            ISBN = new ISBN(isbn) ?? throw new ArgumentNullException(nameof(isbn));

            RentPrice = BookValueConverter.CreateMoneyFromString(rentPrice);
        }
    }
}
