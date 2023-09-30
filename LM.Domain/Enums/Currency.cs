using LM.Domain.SeedWork;

namespace LM.Domain.Enums
{
    public class Currency : Enumeration
    {
        public static Currency RON = new(1, nameof(RON));
        public static Currency USD = new(2, nameof(USD));
        public static Currency EUR = new(3, nameof(EUR));
        public static Currency GBP = new(3, nameof(GBP));

        public Currency(int id, string name)
            : base(id, name)
        {
        }
    }
}
