using LM.Application.Commands;
using LM.Application.Handlers;
using LM.Application.Queries;
using LM.Domain.Aggregates.Book;
using LM.Domain.Aggregates.Identity;
using LM.Domain.Aggregates.Loan;
using LM.Domain.Enums;
using LM.Domain.Repositories;
using Moq;
using System.Collections.Concurrent;
using System.Reflection.Metadata;

namespace LM.Tests.UnitTests
{
    [TestFixture]
    public class CommandHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<ILoanRepository> _loanRepositoryMock;

        private AddBookCommandHandler _addBookHandler;
        private DeleteBookCommandHandler _deleteBookHandler;
        private GetAllBooksQueryHandler _getAllBooksQueryHandler;
        private GetAvailableBooksQueryHandler _getNumberAvailableBookQueryHandler;
        private LoanBookCommandHandler _loanBookCommandHandler;
        private ReturnBookCommandHandler _returnBookCommandHandler;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _bookRepositoryMock = new Mock<IBookRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _loanRepositoryMock = new Mock<ILoanRepository>();

            _addBookHandler = new AddBookCommandHandler(_bookRepositoryMock.Object);
            _deleteBookHandler = new DeleteBookCommandHandler(_bookRepositoryMock.Object);
            _getAllBooksQueryHandler = new GetAllBooksQueryHandler(_bookRepositoryMock.Object);
            _getNumberAvailableBookQueryHandler = new GetAvailableBooksQueryHandler(_bookRepositoryMock.Object);
            _loanBookCommandHandler = new LoanBookCommandHandler(_loanRepositoryMock.Object, _bookRepositoryMock.Object, _userRepositoryMock.Object);
            _returnBookCommandHandler = new ReturnBookCommandHandler(_loanRepositoryMock.Object);
        }

        [Test]
        public void Should_Add_Book()
        {
            // Arrange
            string title = "Some Title";
            string author = "Some Author";
            string isbn = "9781234567890";
            string rentPrice = "10";
            string currency = "RON";

            var command = new AddBookCommand(title, author, isbn, rentPrice, currency);

            // Setup mock to simulate the behavior of the repository
            _bookRepositoryMock.Setup(repo => repo.Add(It.IsAny<Book>())).Verifiable();

            // Act
            var result = _addBookHandler.Handle(command, CancellationToken.None);

            // Assert
            _bookRepositoryMock.Verify(); // Verify that the repository’s AddBook method was called
                                          // Add any additional assertions to check the result or any other side effects.
        }

        [Test]
        public void Should_Delete_Book()
        {
            // Arrange
            string title = "Some Title";
            Author author = new("Some", "Author");
            string isbn = "9781234567890";
            Money money = new(10.23m, Currency.RON);

            // Create a book object
            var book = new Book(title, author, new ISBN(isbn), money);

            // Set up the repository to return the book when GetBookByISBN is called
            _bookRepositoryMock.Setup(repo => repo.FindByISBN(It.IsAny<ISBN>())).Returns(book);

            // Set up the repository to remove the book when RemoveBook is called
            _bookRepositoryMock.Setup(repo => repo.Remove(It.IsAny<ISBN>())).Returns(book);

            // Now prepare to delete the book
            var deleteCommand = new DeleteBookCommand(isbn);

            // Act
            var result = _deleteBookHandler.Handle(deleteCommand, CancellationToken.None).Result;

            // Assert
            _bookRepositoryMock.Verify(repo => repo.Remove(It.IsAny<ISBN>()), Times.Once);
        }

        [Test]
        public void Should_Get_All_Books()
        {
            // Define a list of books
            var books = new List<Book>
            {
                new Book("Title1", new("Author","1"), new ISBN("9781234567890"), new Money(10m, Currency.USD)),
                new Book("Title2", new("Author","2"), new ISBN("9781234567891"), new Money(15m, Currency.EUR)),
                // Add more books as needed
            };

            // Set up the mock repository to return the list of books
            _bookRepositoryMock.Setup(repo => repo.GetAll()).Returns(books.AsQueryable());

            // Act
            var result = _getAllBooksQueryHandler.Handle(new GetAllBooksQuery(), CancellationToken.None).Result;

            // Assert
            Assert.That(result, Has.Count.EqualTo(books.Count)); // Check the number of books returned

            // Optionally, check that the properties of each returned BookDTO match the original books
            for (int i = 0; i < books.Count; i++)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(result[i].Title, Is.EqualTo(books[i].Title));
                    Assert.That(result[i].Author, Is.EqualTo(books[i].Author.GetFormattedName()));
                });
            }

            // Optionally, verify that the GetAllBooks method of the repository was called once
            _bookRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Test]
        public void Should_Get_Number_Available_Books()
        {
            var isbn1 = "9781234567890";
            var book1 = new Book("Title1", new("Author", "1"), new ISBN(isbn1), new Money(10m, Currency.USD));
            book1.AddItem(new Money(9m, Currency.EUR));

            // Define a list of books
            var books = new List<Book>
            {
               book1,
            };

            // Set up the mock repository to return the list of books
            _bookRepositoryMock.Setup(repo => repo.FindByISBN(new ISBN(isbn1))).Returns(book1);

            // Act
            var query = new GetAvailableBooksQuery(isbn1);

            var result = _getNumberAvailableBookQueryHandler.Handle(query, CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void Should_Loan_Book()
        {
            // Arrange
            var mockUser = new User("Bibliotecar1", "hashedPassword", UserRole.Librarian);
            var mockBook = new Book("Sample Book", new Author("John", "Doe"), new ISBN("1234567891011"), new Money(50m, Currency.RON));
            mockBook.AddItem(new Money(50m, Currency.RON)); // Add a book item to mock data

            _userRepositoryMock.Setup(x => x.GetUserByUsername(It.IsAny<string>())).Returns(mockUser);
            _bookRepositoryMock.Setup(x => x.FindByISBN(It.IsAny<ISBN>())).Returns(mockBook);

            var command = new LoanBookCommand("1234567891011", "Cititor1");

            // Act
            _loanBookCommandHandler.Handle(command, CancellationToken.None).Wait();

            // Assert
            _loanRepositoryMock.Verify(x => x.Add(It.Is<Loan>(l => l.UserId == mockUser.Id && l.BookItemId == mockBook.BookItems.First().Id)), Times.Once);
        }

        [Test]
        public async Task Should_Return_Book()
        {
            // Arrange
            var mockUser = new User("Bibliotecar1", "hashedPassword", UserRole.Librarian);
            var bookItemId = Guid.NewGuid();
            var loan = new Loan(bookItemId, mockUser.Id);

            _loanRepositoryMock.Setup(x => x.FindByBookItemId(bookItemId)).Returns(loan);

            var returnDate = new DateTime(2023, 1, 1); // fixed date
            var command = new ReturnBookCommand(bookItemId.ToString(), BookQualityState.Good.Name, returnDate.ToString("o"));

            // Act
            await _returnBookCommandHandler.Handle(command, default);

            // Assert
            Assert.AreEqual(returnDate, loan.ReturnDate);
        }
    }
}
