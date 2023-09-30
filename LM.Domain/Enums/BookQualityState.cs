using LM.Domain.SeedWork;

namespace LM.Domain.Enums
{
    public class BookQualityState : Enumeration
    {
        public static BookQualityState New = new(1, nameof(New));
        public static BookQualityState LikeNew = new(2, nameof(LikeNew)); 
        public static BookQualityState Good = new(3, nameof(Good)); 
        public static BookQualityState Acceptable = new(4, nameof(Acceptable)); 
        public static BookQualityState Damaged = new(5, nameof(Damaged)); 
        public static BookQualityState Repaired = new(6, nameof(Repaired)); 
        public static BookQualityState Unusable = new(7, nameof(Unusable));

        public BookQualityState(int id, string name)
            : base(id, name)
        {
        }
    }
}
