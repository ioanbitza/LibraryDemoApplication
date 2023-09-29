using LM.ConsoleApplication.DTOs;
using MediatR;

namespace LM.Application.Queries
{
    public class GetAllBooksQuery : IRequest<List<BookDTO>>
    {
    }
}
