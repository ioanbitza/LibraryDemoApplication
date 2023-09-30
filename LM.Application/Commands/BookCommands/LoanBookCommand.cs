using LM.Domain.Aggregates.Book;
using MediatR;

namespace LM.Application.Commands
{
    public class LoanBookCommand : IRequest
    {
        public ISBN ISBN { get; }
        public string Username { get; }
        public DateTime LoanDate { get; }

        public LoanBookCommand(string isbn, string username, string? loanDate = null)
        {
            ISBN = new ISBN(isbn) ?? throw new ArgumentNullException(nameof(isbn));
            Username = username;
            if (loanDate != null)
            {
                if (!DateTime.TryParse(loanDate, out DateTime parsedReturnDate))
                {
                    throw new ArgumentException("Invalid Loan Date format.", nameof(loanDate));
                }

                LoanDate = parsedReturnDate;
            }
            else
            {
                LoanDate = DateTime.UtcNow;
            }
        }
    }
}
