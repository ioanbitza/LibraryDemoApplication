using LM.Domain.Aggregates.Loan;

namespace LM.Domain.Repositories
{
    public interface ILoanRepository
    {
        void Add(Loan loan);
        void Update(Loan loan);
        Loan FindByBookItemId(Guid bookItemId);
        IQueryable<Loan> GetAllByUserId(Guid userId);
        Loan Remove(Guid loanId);
    }
}
