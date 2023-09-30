using LM.Application.Commands;
using LM.Domain.Aggregates.Book;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        private readonly IBookRepository _bookRepository;

        public AddBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book(request.Title, request.Author, request.ISBN, request.RentPrice);
            _bookRepository.Add(book);
            return Unit.Task;
        }
    }
}
