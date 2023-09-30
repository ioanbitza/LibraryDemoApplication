using LM.Domain.SeedWork;
using MediatR;

namespace LM.Infrastructure.Service
{
    public class DomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchEventsFor(Entity aggregate)
        {
            if (aggregate.DomainEvents == null || !aggregate.DomainEvents.Any())
            {
                return;
            }

            foreach (var domainEvent in aggregate.DomainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

            aggregate.ClearDomainEvents(); // Clears the domain events to ensure they're not dispatched multiple times.
        }
    }
}
