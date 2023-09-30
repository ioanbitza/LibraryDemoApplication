using LM.Domain.SeedWork;

namespace LM.Domain.Aggregates.Book
{
    public class Author : ValueObject // It should be an Entity if we would had a Database.
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? MiddleName { get; private set; }

        public Author(string firstName, string lastName, string? middleName = null) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName ?? null;
        }

        protected override IEnumerable<string> GetEqualityComponents()
        {
            yield return GetFormattedName();
        }

        public string GetFormattedName()
        {
            return MiddleName != null
                ? $"{FirstName} {MiddleName} {LastName}"
                : $"{FirstName} {LastName}";
        }
    }
}
