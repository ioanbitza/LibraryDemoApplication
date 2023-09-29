using LM.Application.Queries;
using LM.ConsoleApplication.DTOs;
using LM.Domain.Repositories;
using LM.Infrastructure.Repositories;
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
            var books = _bookRepository.GetAllBooks().ToList();
            var bookDTOs = new List<BookDTO>();

            foreach (var book in books)
            {
                bookDTOs.Add(new BookDTO(book));
            }

            return Task.FromResult(bookDTOs);
        }
    }
}