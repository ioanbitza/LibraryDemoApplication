using LM.Domain.SeedWork;

namespace LM.Domain.Aggregates.Book
{
    public class ISBN : ValueObject
    {
        private readonly long _value;

        public ISBN(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            // TODO: Add more ISBN validation logic here.
            if (value.Length != 13) throw new ArgumentException("The ISBN lenght should have 13 digits.");
            if (!long.TryParse(value, out var valueint)) throw new ArgumentException("The ISBN should be a number.");
            _value = valueint;
        }

        public ISBN(long value)
        {
            _value = value;
        }

        public long Value => _value;

        public override bool Equals(object obj)
        {
            if (obj is ISBN isbn)
            {
                return _value == isbn._value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
