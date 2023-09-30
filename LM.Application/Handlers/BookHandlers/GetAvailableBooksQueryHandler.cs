using LM.Application.Queries;
using LM.Domain.Aggregates.Book;
using LM.Domain.Repositories;
using MediatR;

namespace LM.Application.Handlers
{
    public class GetAvailableBooksQueryHandler : IRequestHandler<GetAvailableBooksQuery, int>
    {
        private readonly IBookRepository _bookRepository;

        public GetAvailableBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<int> Handle(GetAvailableBooksQuery request, CancellationToken cancellationToken)
        {
            var book = _bookRepository.FindByISBN(new ISBN(request.ISBN));
            var availables = book.Availables;
            return Task.FromResult(availables);
        }
    }
}

