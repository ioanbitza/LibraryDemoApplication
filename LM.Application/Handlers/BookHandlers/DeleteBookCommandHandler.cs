using LM.Application.Commands;
using LM.ConsoleApplication.DTOs;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, BookDTO>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<BookDTO> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.RemoveBook(request.ISBN);
            var bookDTO = new BookDTO(book);
            return Task.FromResult(bookDTO);
        }
    }
}
