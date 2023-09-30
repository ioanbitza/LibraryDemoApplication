using LM.Domain.Aggregates.Book;
using LM.Domain.Enums;
using LM.Domain.SeedWork;
using MediatR;

namespace LM.Application.Commands
{
    public class ReturnBookCommand : IRequest
    {
        public Guid BookItemId { get; }
        public DateTime ReturnDate { get; }
        public BookQualityState BookQuality { get; }
        public ReturnBookCommand(string bookItemId, string bookQuality, string? returnDate = null)
        {
            BookQuality = Enumeration.FromDisplayName<BookQualityState>(bookQuality) ?? throw new ArgumentNullException("The quality book name doesn't exist.");

            if (!Guid.TryParse(bookItemId, out Guid parsedBookItemId))
            {
                throw new ArgumentException("Invalid Book Item ID format.", nameof(bookItemId));
            }

            BookItemId = parsedBookItemId;

            if (returnDate != null)
            {
                if (!DateTime.TryParse(returnDate, out DateTime parsedReturnDate))
                {
                    throw new ArgumentException("Invalid Return Date format.", nameof(returnDate));
                }

                ReturnDate = parsedReturnDate;
            }
            else
            {
                ReturnDate = DateTime.UtcNow;
            }
        }
    }
}
