namespace NetCore.Domain.Exceptions;

public class EntityNotFoundException : Exception
{
	public EntityNotFoundException(string request, Guid id) :
		base($"[{request}]-[{id}] is not found")
	{

	}
}
