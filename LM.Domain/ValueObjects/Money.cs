using LM.Domain.Enums;

namespace LM.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public CurrencyEnum Currency { get; private set; }

        public Money(decimal amount, CurrencyEnum currency)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
            Amount = amount;
            Currency = currency;
        }

        // ... other methods, if necessary
    }
}
