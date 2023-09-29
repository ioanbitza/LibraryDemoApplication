using LM.Application.Commands;
using LM.Application.Handlers;
using LM.Application.Queries;
using LM.Domain.Aggregates.Book;
using LM.Domain.Aggregates.Identity;
using LM.Domain.DomainServices;
using LM.Domain.Enums;
using LM.Domain.Repositories;
using LM.Domain.ValueObjects;
using Moq;

namespace LM.Tests.UnitTests
{
    [TestFixture]
    public class CommandHandlerTests
    {
        private Mock<IBookRepository> _bookRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IIdentityService> _identityServiceMock;

        private AddBookCommandHandler _addBookHandler;
        private DeleteBookCommandHandler _deleteBookHandler;
        private GetAllBooksQueryHandler _getAllBooksQueryHandler;
        private GetNumberAvailableBookQueryHandler _getNumberAvailableBookQueryHandler;
        private LoanBookCommandHandler _loanBookCommandHandler;
        private ReturnBookCommandHandler _returnBookCommandHandler;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _bookRepositoryMock = new Mock<IBookRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _identityServiceMock = new Mock<IIdentityService>();

            _addBookHandler = new AddBookCommandHandler(_bookRepositoryMock.Object);
            _deleteBookHandler = new DeleteBookCommandHandler(_bookRepositoryMock.Object);
            _getAllBooksQueryHandler = new GetAllBooksQueryHandler(_bookRepositoryMock.Object);
            _getNumberAvailableBookQueryHandler = new GetNumberAvailableBookQueryHandler(_bookRepositoryMock.Object);
            _loanBookCommandHandler = new LoanBookCommandHandler(_bookRepositoryMock.Object, _userRepositoryMock.Object);
            _returnBookCommandHandler = new ReturnBookCommandHandler(_bookRepositoryMock.Object);
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
            _bookRepositoryMock.Setup(repo => repo.AddBook(It.IsAny<Book>())).Verifiable();

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
            string author = "Some Author";
            string isbn = "9781234567890";
            string rentPrice = "10.23";
            string currency = "RON";

            // Create a book object
            var book = new Book(title, author, new ISBN(isbn), new Money(decimal.Parse(rentPrice), Enum.Parse<CurrencyEnum>(currency)));

            // Set up the repository to return the book when GetBookByISBN is called
            _bookRepositoryMock.Setup(repo => repo.GetBookByISBN(It.IsAny<ISBN>())).Returns(book);

            // Set up the repository to remove the book when RemoveBook is called
            _bookRepositoryMock.Setup(repo => repo.RemoveBook(It.IsAny<ISBN>())).Returns(book);

            // Now prepare to delete the book
            var deleteCommand = new DeleteBookCommand(isbn);

            // Act
            var result = _deleteBookHandler.Handle(deleteCommand, CancellationToken.None).Result;

            // Assert
            _bookRepositoryMock.Verify(repo => repo.RemoveBook(It.IsAny<ISBN>()), Times.Once);
        }

        [Test]
        public void Should_Get_All_Books()
        {
            // Define a list of books
            var books = new List<Book>
            {
                new Book("Title1", "Author1", new ISBN("9781234567890"), new Money(10m, CurrencyEnum.USD)),
                new Book("Title2", "Author2", new ISBN("9781234567891"), new Money(15m, CurrencyEnum.EUR)),
                // Add more books as needed
            };

            // Set up the mock repository to return the list of books
            _bookRepositoryMock.Setup(repo => repo.GetAllBooks()).Returns(books.AsQueryable());

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
                    Assert.That(result[i].Author, Is.EqualTo(books[i].Author));
                });
            }

            // Optionally, verify that the GetAllBooks method of the repository was called once
            _bookRepositoryMock.Verify(repo => repo.GetAllBooks(), Times.Once);
        }

        [Test]
        public void Should_Get_Number_Available_Books()
        {

            // Define a list of books
            var books = new List<Book>
            {
                new Book("Title1", "Author1", new ISBN("9781234567890"), new Money(10m, CurrencyEnum.USD)),
                new Book("Title2", "Author2", new ISBN("9781234567891"), new Money(15m, CurrencyEnum.EUR)),
                // Add more books as needed
            };

            // Set up the mock repository to return the list of books
            _bookRepositoryMock.Setup(repo => repo.GetAllBooks()).Returns(books.AsQueryable());

            // Act
            var query = new GetNumberAvailableBookQuery("Title1", "Author1");

            var result = _getNumberAvailableBookQueryHandler.Handle(query, CancellationToken.None).Result;

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Should_Loan_Book()
        {
            // Arrange
            var isbnValue = "9781234567890";
            var isbn = new ISBN(isbnValue);
            var username = "TestUser";
            var loanDate = DateTime.Now;

            var user = new User(username, "TestPassword", UserRoleEnum.Librarian);
            var book = new Book("Test Title", "Test Author", isbn, new Money(100m, CurrencyEnum.USD));

            _bookRepositoryMock.Setup(repo => repo.GetBookByISBN(isbn)).Returns(book);
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

            var command = new LoanBookCommand(isbnValue, username);

            // Act
            _loanBookCommandHandler.Handle(command, CancellationToken.None).Wait();
            Assert.Multiple(() =>
            {

                // Assert
                Assert.That(book.IsAvailable, Is.False, "Book should not be available.");
                Assert.That(book.Loans, Is.Not.Empty, "No loans found.");
            });
            var loan = book.Loans.LastOrDefault(l => l.UserId == user.Id);
            Assert.That(loan, Is.Not.Null, "Loan for the given user ID not found.");
        }

        [Test]
        public void Should_Return_Book()
        {

            // Arrange
            var isbnValue = "9781234567890";
            var isbn = new ISBN(isbnValue);
            var username = "TestUser";
            var loanDate = DateTime.Now;

            var user = new User(username, "TestPassword", UserRoleEnum.Librarian);
            var book = new Book("Test Title", "Test Author", isbn, new Money(100m, CurrencyEnum.USD));

            _bookRepositoryMock.Setup(repo => repo.GetBookByISBN(isbn)).Returns(book);
            _userRepositoryMock.Setup(repo => repo.GetUserByUsername(username)).Returns(user);

            var loanCommand = new LoanBookCommand(isbnValue, username);
            var returnCommand = new ReturnBookCommand(isbnValue);

            // Act
            _loanBookCommandHandler.Handle(loanCommand, CancellationToken.None).Wait();
            _returnBookCommandHandler.Handle(returnCommand, CancellationToken.None).Wait();

            Assert.Multiple(() =>
            {

                // Assert
                Assert.That(book.IsAvailable, Is.True, "Book should be available.");
                Assert.That(book.Loans, Is.Not.Empty, "No loans found.");
            });
            var loan = book.Loans.LastOrDefault(l => l.UserId == user.Id);
            Assert.That(loan, Is.Not.Null, "Loan for the given user ID not found.");
        }

    }
}
