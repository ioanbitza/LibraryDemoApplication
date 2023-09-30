using LM.Application.DTOs;
using LM.Domain.Aggregates.Book;
using MediatR;

namespace LM.Application.Commands
{
    public class DeleteBookCommand : IRequest<BookDTO>
    {
        public ISBN ISBN { get; }

        public DeleteBookCommand(string isbn)
        {
            ISBN = new ISBN(isbn);
        }
    }
}
