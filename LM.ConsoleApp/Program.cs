using LM.Application.Handlers;
using LM.ConsoleApp;
using LM.Domain.DomainEvents;
using LM.Domain.Repositories;
using LM.Infrastructure.Repositories;
using LM.Infrastructure.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(BookLoanedEventHandler)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(BookLoanedEvent)));

            // Register Application services
            services.AddTransient<GetAllBooksQueryHandler>();
            services.AddTransient<AddBookCommandHandler>();
            services.AddTransient<DeleteBookCommandHandler>();
            services.AddTransient<GetAvailableBooksQueryHandler>();
            services.AddTransient<LoanBookCommandHandler>();
            services.AddTransient<ReturnBookCommandHandler>();

            services.AddTransient<BookLoanedEventHandler>();
            services.AddTransient<BookReturnedEventHandler>();

            services.AddTransient<GetUserByUsernameQueryHandler>();
            services.AddTransient<LoginCommandHandler>();

            services.AddTransient<DomainEventDispatcher>();

            // Register Infrastructure services
            // I tried to simulate a HTTP Requests so that why we have an Interface for repositories
            // instead using a Singleton for them which was more appropiate for how we are using them
            services.AddScoped<IBookRepository, InMemoryBookRepository>();
            services.AddScoped<IUserRepository, InMemoryUserRepository>();
            services.AddScoped<ILoanRepository, InMemoryLoanRepository>();

            // Register UI
            services.AddTransient<ConsoleUI>();
        }).Build(); ;

// Run the application
var consoleUI = host.Services.GetRequiredService<ConsoleUI>();
await consoleUI.RunAsync();
