using LM.Application.Queries;
using LM.ConsoleApplication.DTOs;
using LM.Domain.Repositories;
using LM.Infrastructure.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class GetNumberAvailableBookQueryHandler : IRequestHandler<GetNumberAvailableBookQuery, int>
    {
        private readonly IBookRepository _bookRepository;

        public GetNumberAvailableBookQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<int> Handle(GetNumberAvailableBookQuery request, CancellationToken cancellationToken)
        {
            var booksnumber = _bookRepository.GetAllBooks().Where(book => book.IsAvailable && book.Author.ToLower() == request.Author.ToLower() && book.Title.ToLower() == request.Title.ToLower()).Count();

            return Task.FromResult(booksnumber);
        }
    }
}

