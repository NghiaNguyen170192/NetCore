namespace NetCore.Domain.SharedKernel;

public abstract class Entity
{
	private int? _requestedHashCode;

	//private List<INotification> _domainEvents=  new();
	//public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();

	public virtual Guid Id { get; set; }

	public DateTime CreatedDate { get; set; }

	public Guid CreatedBy { get; set; }

	public DateTime ModifiedDate { get; set; }

	public Guid ModifiedBy { get; set; }

	//protected void AddDomainEvent(INotification eventItem)
	//{
	//	_domainEvents.Add(eventItem);
	//}

	//protected void RemoveDomainEvent(INotification eventItem)
	//{
	//	_domainEvents?.Remove(eventItem);
	//}

	//public void ClearDomainEvents()
	//{
	//	_domainEvents?.Clear();
	//}

	protected bool IsTransient()
	{
		return Id == default;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is Entity))
			return false;

		if (ReferenceEquals(this, obj))
			return true;

		if (GetType() != obj.GetType())
			return false;

		var item = (Entity)obj;

		if (item.IsTransient() || IsTransient())
			return false;
		
		return item.Id == Id;
	}

	public override int GetHashCode()
	{
		if (!IsTransient())
		{
			if (!_requestedHashCode.HasValue)
				_requestedHashCode = Id.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

			return _requestedHashCode.Value;
		}

		return base.GetHashCode();
	}

	public static bool operator ==(Entity left, Entity right)
	{
		if (Equals(left, null))
			return (Equals(right, null));
		
		return left.Equals(right);
	}

	public static bool operator !=(Entity left, Entity right)
	{
		return !(left == right);
	}
}