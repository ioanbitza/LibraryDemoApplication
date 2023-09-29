using LM.Domain.Aggregates.Book;

namespace LM.ConsoleApplication.DTOs
{
    public class LoanDTO
    {
        public Guid Id { get; }
        public DateTime LoanDate { get; }
        public DateTime DueDate { get; }
        public DateTime? ReturnDate { get; }

        public LoanDTO(Loan loan)
        {
            Id = loan.Id;
            LoanDate = loan.LoanDate;
            DueDate = loan.DueDate;
            ReturnDate = loan.ReturnDate;
        }
    }
}
