using LM.Application.Handlers;
using LM.Application.Services;
using LM.ConsoleApp;
using LM.Domain.DomainServices;
using LM.Domain.Repositories;
using LM.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace LM.UI.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            // Run the application
            var consoleUI = host.Services.GetRequiredService<ConsoleUI>();
            await consoleUI.RunAsync();
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
                    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly));

                    // Register Application services
                    services.AddTransient<GetAllBooksQueryHandler>();
                    services.AddTransient<AddBookCommandHandler>();
                    services.AddTransient<DeleteBookCommandHandler>();
                    services.AddTransient<GetNumberAvailableBookQueryHandler>();
                    services.AddTransient<LoanBookCommandHandler>();
                    services.AddTransient<ReturnBookCommandHandler>();

                    services.AddTransient<GetUserByUsernameQueryHandler>();
                    services.AddTransient<LoginCommandHandler>();

                    // Register Infrastructure services
                    services.AddScoped<IBookRepository, InMemoryBookRepository>();
                    services.AddScoped<IUserRepository, InMemoryUserRepository>();


                    services.AddScoped<IIdentityService, IdentityService>();

                    // Register UI
                    services.AddTransient<ConsoleUI>();
                });
        }
    }
}