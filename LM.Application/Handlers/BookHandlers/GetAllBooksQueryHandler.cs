using LM.Application.DTOs;
using LM.Application.Queries;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookDTO>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<List<BookDTO>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = _bookRepository.GetAll();
            var bookDTOs = books.Select(book => new BookDTO(book)).ToList();

            return Task.FromResult(bookDTOs);
        }
    }
}