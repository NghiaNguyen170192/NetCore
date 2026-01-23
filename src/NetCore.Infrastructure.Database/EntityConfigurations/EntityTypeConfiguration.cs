using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetCore.Domain.SharedKernel;

namespace NetCore.Infrastructure.Database.EntityConfigurations;

public class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
{
	public virtual void Configure(EntityTypeBuilder<T> builder)
	{
		builder.HasKey(entity => entity.Id);
		builder.HasIndex(entity => entity.CreatedDate);
		builder.HasIndex(entity => entity.ModifiedDate);

		Configure(builder);
	}
}