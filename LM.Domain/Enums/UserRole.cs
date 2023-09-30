using LM.Domain.SeedWork;

namespace LM.Domain.Enums
{
    public class UserRole : Enumeration
    {
        public static UserRole Reader = new(1, nameof(Reader));
        public static UserRole Librarian = new(2, nameof(Librarian));

        public UserRole(int id, string name)
            : base(id, name)
        {
        }
    }
}
