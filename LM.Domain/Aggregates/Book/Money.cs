using LM.Domain.Enums;
using LM.Domain.SeedWork;

namespace LM.Domain.Aggregates.Book
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public Currency Currency { get; private set; }

        public Money(decimal amount, Currency currency)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Amount = amount;
            Currency = currency;
        }

        protected override IEnumerable<string> GetEqualityComponents()
        {
            yield return GetFormattedMoney();
        }

        public string GetFormattedMoney()
        {
            return $"{Amount} {Currency}";
        }
    }
}
