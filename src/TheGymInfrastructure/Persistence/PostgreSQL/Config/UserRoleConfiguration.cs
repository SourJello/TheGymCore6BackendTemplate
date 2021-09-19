using TheGymInfrastructure.Persistence.PostgreSQL.Config.Common;
using TheGymDomain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace TheGymInfrastructure.Persistence.PostgreSQL.Config
{
    internal class UserRoleConfiguration : EntityConfiguration<UserRole>
    {
        //TODO: Do I have to specify tablename or will it default to user
        private const string TableName = "UserRole";
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.ToTable(TableName);

            builder.Property(x => x.RoleName)
                .HasColumnType("varchar(256)")
                .IsRequired();
        }
        public class UserValidator : EntityValidator<UserRole>
        {
            public UserValidator()
            {
                RuleFor(x => x.RoleName)
                    .Length(1, 256)
                    .NotEmpty()
                    .WithErrorCode("Role name error");

            }
        }
    }
}
