using NetCore.Domain.SharedKernel;

namespace NetCore.Infrastructure.Database.Extensions;

public static class MediatorExtension
{
	//public static async Task DispatchDomainEventsAsync(this IMediator mediator, ApplicationDatabaseContext context)
	//{
	//	var domainEntities = context.ChangeTracker
	//		.Entries<Entity>()
	//		.Where(x => x.Entity.DomainEvents.Any())
	//		.ToList();

	//	var domainEvents = domainEntities
	//		.SelectMany(x => x.Entity.DomainEvents)
	//		.ToList();

	//	domainEntities
	//		.ForEach(x => x.Entity.ClearDomainEvents());

	//	foreach (var domainEvent in domainEvents)
	//		await mediator.Publish(domainEvent);
	//}
}