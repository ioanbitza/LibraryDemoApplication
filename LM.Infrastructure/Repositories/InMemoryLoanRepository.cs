using LM.Domain.Aggregates.Loan;
using LM.Domain.Repositories;
using System.Collections.Concurrent;
using MediatR;
using LM.Infrastructure.Service;

namespace LM.Infrastructure.Repositories
{
    public class InMemoryLoanRepository : ILoanRepository
    {
        private readonly ConcurrentDictionary<Guid, Loan> _loans = new();
        private readonly DomainEventDispatcher _domainEventDispatcher;
        public InMemoryLoanRepository(DomainEventDispatcher domainEventDispatcher)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public void Add(Loan loan)
        {
            _loans.TryAdd(loan.Id, loan);
            _domainEventDispatcher.DispatchEventsFor(loan);
        }

        public void Update(Loan updatedloan)
        {
            if (updatedloan == null) throw new ArgumentNullException(nameof(updatedloan));

            if (_loans.TryGetValue(updatedloan.Id, out var existingloan))
            {
                _loans.TryUpdate(updatedloan.Id, updatedloan, existingloan);

                _domainEventDispatcher.DispatchEventsFor(updatedloan);
            }
            else
            {
                throw new KeyNotFoundException($"No loan found with Id {updatedloan.Id}.");
            }
        }

        public Loan FindByBookItemId(Guid bookItemId)
        {
            var loan = _loans.Values.SingleOrDefault(l => l.BookItemId == bookItemId);
            return loan is null ? throw new KeyNotFoundException("Loan not found.") : loan;
        }

        public IQueryable<Loan> GetAllByUserId(Guid userId)
        {
            var loans = _loans.Values.Where(l => l.UserId == userId);

            return loans.AsQueryable();
        }

        public Loan Remove(Guid loanId)
        {
            _loans.TryRemove(loanId, out var loan);
            if (loan is null) throw new KeyNotFoundException("Loan not found.");
            return loan;
        }
    }
}
