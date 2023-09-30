using LM.Domain.Aggregates.Book;
using LM.Domain.Enums;
using LM.Domain.SeedWork;

namespace LM.Application.Utilities
{
    public static class BookValueConverter
    {
        public static Author CreateAuthorFromString(string authorName)
        {
            if (string.IsNullOrWhiteSpace(authorName))
                throw new ArgumentException("Name cannot be null or whitespace", nameof(authorName));

            var nameParts = authorName.Split(' ');

            return nameParts.Length switch
            {
                2 => new Author(nameParts[0], nameParts[1]),
                3 => new Author(nameParts[0], nameParts[2], nameParts[1]),
                _ => throw new ArgumentException($"Cannot interpret name: {authorName}", nameof(authorName)),
            };
        }

        public static string CreateStringFromAuthor(Author author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            if (string.IsNullOrWhiteSpace(author.FirstName) || string.IsNullOrWhiteSpace(author.LastName))
                throw new ArgumentException("Author's first or last name cannot be null or whitespace.");

            return author.GetFormattedName();
        }

        public static Money CreateMoneyFromString(string money)
        {
            if (string.IsNullOrWhiteSpace(money))
                throw new ArgumentException("Money cannot be null or whitespace", nameof(money));

            var moneyParts = money.Split(' ');
            if (!decimal.TryParse(moneyParts[0], out var amount)) throw new ArgumentException("Amount is not a number");
            var currency = Enumeration.FromDisplayName<Currency>(moneyParts[1]);
            return new Money(amount, currency);
        }

        public static string CreateStringFromMoney(Money money)
        {
            if (money == null)
                throw new ArgumentNullException(nameof(money));

            return money.GetFormattedMoney();
        }
    }
}
