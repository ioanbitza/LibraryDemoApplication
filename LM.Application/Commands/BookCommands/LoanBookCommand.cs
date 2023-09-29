using LM.Domain.Aggregates.Identity;
using LM.Domain.ValueObjects;
using MediatR;

namespace LM.Application.Commands
{
    public class LoanBookCommand : IRequest
    {
        public ISBN ISBN { get; }
        public string Username { get; }
        public DateTime LoanDate { get; }

        public LoanBookCommand(string isbn, string username, DateTime? returnDate = null)
        {
            ISBN = new ISBN(isbn) ?? throw new ArgumentNullException(nameof(isbn));
            Username = username;
            LoanDate = returnDate ?? DateTime.Now;
        }
    }
}
