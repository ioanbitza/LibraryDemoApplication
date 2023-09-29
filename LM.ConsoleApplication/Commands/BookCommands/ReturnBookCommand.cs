using LM.Domain.ValueObjects;
using MediatR;

namespace LM.Application.Commands
{
    public class ReturnBookCommand : IRequest
    {
        public ISBN ISBN { get; }
        public DateTime ReturnDate { get; }

        public ReturnBookCommand(string isbn, DateTime? returnDate = null)
        {
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentNullException(nameof(isbn));

            ISBN = new ISBN(isbn);
            ReturnDate = returnDate ?? DateTime.Now;
        }
    }
}
