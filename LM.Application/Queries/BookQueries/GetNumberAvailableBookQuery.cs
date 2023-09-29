using LM.ConsoleApplication.DTOs;
using MediatR;

namespace LM.Application.Queries
{
    public class GetNumberAvailableBookQuery : IRequest<int>
    {
        public string Author { get; }
        public string Title { get; }

        public GetNumberAvailableBookQuery(string title, string author)
        {
            Author = author;
            Title = title;
        }
    }
}
