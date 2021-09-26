using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TheGymDomain.Models.Common;

namespace TheGymInfrastructure.Persistence.PostgreSQL.Config.Common
{
    internal abstract class EntityConfiguration<TBase> : IEntityTypeConfiguration<TBase>
        where TBase : Entity
    {
        public virtual void Configure(EntityTypeBuilder<TBase> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedByUserId);

            builder.Property(x => x.CreatedDate);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.ModifiedByUserId);

            builder.Property(x => x.ModifiedDate);
        }
        public abstract class EntityValidator<TBase> : AbstractValidator<TBase>
            where TBase : Entity
        {
            public EntityValidator()
            {

            }
        }
    }
}
