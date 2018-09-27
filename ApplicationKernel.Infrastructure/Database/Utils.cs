using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationKernel.Infrastructure.Database
{
    public static class Utils
    {
        public static EntityTypeBuilder<TEntity> MakePropertyRequired<TEntity, TProperty>(
            this EntityTypeBuilder<TEntity> entityTypeBuilder,
            Expression<Func<TEntity, TProperty>> property) where TEntity : class
        {
            entityTypeBuilder.Property(property).IsRequired();
            return entityTypeBuilder;
        }

        public static void DisableCascadeDeletions(this ModelBuilder modelBuilder) 
        {
            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
