using LM.ConsoleApplication.DTOs;
using LM.Domain.ValueObjects;
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
