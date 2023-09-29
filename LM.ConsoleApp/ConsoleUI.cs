﻿using LM.Application.Commands;
using LM.Application.Queries;
using LM.ConsoleApplication.DTOs;
using LM.Domain.Aggregates.Identity;
using LM.Domain.Enums;
using LM.Domain.ValueObjects;
using MediatR;

namespace LM.ConsoleApp
{
    public class ConsoleUI
    {
        private readonly IMediator _mediator;
        private User _currentUser;

        public ConsoleUI(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                if (_currentUser == null)
                {
                    Console.Write("Username: ");
                    var username = Console.ReadLine();

                    Console.Write("Password: ");
                    var password = Console.ReadLine();

                    _currentUser = await _mediator.Send(new LoginCommand(username, password));

                    if (_currentUser == null)
                    {
                        Console.WriteLine("Invalid credentials. Please try again.");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Hello {_currentUser.Username}, How can I help you today?");
                        Console.WriteLine($"Be aware that you have role: {_currentUser.Role}");
                    }
                }

                // After login, show options to the user based on their role.
                // Keep showing options until the user chooses to exit.
                while (_currentUser != null)
                {
                    switch (_currentUser.Role)
                    {
                        case UserRoleEnum.Librarian:
                            {
                                Console.WriteLine();
                                Console.WriteLine("1. Register a new BOOK.");
                                Console.WriteLine("2. Get All BOOKS.");
                                Console.WriteLine("3. Get the number of available specific BOOK.");
                                Console.WriteLine("4. Loan a BOOK.");
                                Console.WriteLine("5. Return a BOOK.");
                                Console.WriteLine("6. Delete a BOOK.");
                                Console.WriteLine("7. Exit");
                                Console.Write("Select an option: ");

                                var option = Console.ReadLine();

                                switch (option)
                                {
                                    case "1":
                                        await AddNewBook();
                                        break;
                                    case "2":
                                        await GetAllBooks();
                                        break;
                                    case "3":
                                        await GetNumberOfAvailableBook();
                                        break;
                                    case "4":
                                        await LoanABook();
                                        break;
                                    case "5":
                                        await ReturnBook();
                                        break;
                                    case "6":
                                        await DeleteBook();
                                        break;
                                    case "7":
                                        _currentUser = null; // Log out
                                        break;
                                    default:
                                        Console.WriteLine("Invalid option. Please try again.");
                                        break;
                                }

                                Console.WriteLine("Clean console(Y/N)?");
                                var cleanConsole = Console.ReadLine();
                                Console.ReadKey(true);  // 'true' means don't display the pressed key
                                if ( cleanConsole is null or "Y" or "y" )
                                {
                                    Console.Clear();
                                }

                                // If user chose to exit, break from the inner while loop.
                                if (_currentUser == null)
                                    break;

                                break;
                            }
                        case UserRoleEnum.Reader:
                            {
                                // TO DO
                                break;
                            }
                    }
                }
            }
        }

        private async Task AddNewBook()
        {
            Console.WriteLine();
            Console.WriteLine("What is the title of the book?");
            var title = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("What is the author of the book?");
            var author = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("What is the book ISBN?");
            var isbn = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("What is the rent price of the book?");
            var rentPrice = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("What is the currency?");
            var currency = Console.ReadLine();

            var command = new AddBookCommand(title, author, isbn, rentPrice, currency);

            await _mediator.Send(command);
            Console.WriteLine();
            Console.WriteLine("The book has been succesfully registered.");
        }

        private async Task GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var books = await _mediator.Send(query);

            Console.WriteLine();
            Console.WriteLine("All Books:");
            foreach (var book in books)
            {
                Console.WriteLine($"|- ISBN:         {book.ISBN}");
                Console.WriteLine($"|  Title:        {book.Title}");
                Console.WriteLine($"|  Author:       {book.Author}");
                Console.WriteLine($"|  Availability: {book.IsAvailable}");
                Console.WriteLine($"|_ Rent price:   {book.RentPrice}");
                Console.WriteLine();
            }
        }

        private async Task GetNumberOfAvailableBook()
        {

            Console.WriteLine();
            Console.WriteLine("What is the book author");
            var author = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("What is the book title");
            var title = Console.ReadLine();

            var query = new GetNumberAvailableBookQuery(title, author);
            var result = await _mediator.Send(query);

            Console.WriteLine();
            Console.WriteLine($"Available Books with Title: {title}, Author: {author} are {result}");
        }

        private async Task LoanABook()
        {
            Console.WriteLine();
            Console.WriteLine("What is the ISBN of the book you want to loan?");
            var isbn = Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Who is booking? Type his/her username:");
            var username = Console.ReadLine();

            var command = new LoanBookCommand(isbn, username);

            await _mediator.Send(command);
            Console.WriteLine();
            Console.WriteLine("Thank you for returning the book.");
        }

        private async Task ReturnBook()
        {
            Console.WriteLine();
            Console.WriteLine("What is the ISBN of the book you want to RETURN?");
            var isbn = Console.ReadLine();

            var command = new ReturnBookCommand(isbn);

            await _mediator.Send(command);

            Console.WriteLine();
            Console.WriteLine("Thank you for returning the book.");
        }

        private async Task DeleteBook()
        {
            Console.WriteLine();
            Console.WriteLine("What is the ISBN of the book you want to DELETE?");
            var isbn = Console.ReadLine();

            var command = new DeleteBookCommand(isbn);

            BookDTO result = await _mediator.Send(command);

            Console.WriteLine();
            Console.WriteLine($"The book with the author {result.Author} and the title {result.Title} has been deleted.");
        }
    }
}
