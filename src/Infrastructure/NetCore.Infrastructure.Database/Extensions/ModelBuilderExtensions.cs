using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace NetCore.Infrastructure.Database.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SetDefaultValueTableName(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes().Where(x => x.ClrType.GetCustomAttribute(typeof(TableAttribute)) != null))
            {
                var entityClass = entity.ClrType;
                var tableAttribute = entityClass.GetCustomAttribute(typeof(TableAttribute)) as TableAttribute;

                modelBuilder.Entity(entityClass).ToTable(tableAttribute.Name);
                var properties = entityClass.GetProperties().Where(p => 
                    (p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(Guid)) 
                    && p.CustomAttributes.Any(a => a.AttributeType == typeof(DatabaseGeneratedAttribute) && !p.CustomAttributes.Any(c => c.AttributeType == typeof(DefaultValueAttribute))));

                foreach (var property in )
                {
                    var defaultValueSql = "GetDate()";
                    if (property.PropertyType == typeof(Guid))
                    {
                        defaultValueSql = "newsequentialid()";
                    }

                    modelBuilder.Entity(entityClass).Property(property.Name).HasDefaultValueSql(defaultValueSql);
                }

                foreach (var property in entityClass.GetProperties().Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(DefaultValueAttribute))))
                {
                    var attribute = property.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute;

                    modelBuilder.Entity(entityClass).Property(property.Name).HasDefaultValueSql(attribute.Value.ToString());
                }
            }
        }
    }
}
