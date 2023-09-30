using LM.Application.DTOs;
using MediatR;

namespace LM.Application.Queries
{
    public class GetAvailableBooksQuery : IRequest<int>
    {
        public string ISBN { get; }

        public GetAvailableBooksQuery(string isbn)
        {
            ISBN = isbn;
        }
    }
}
