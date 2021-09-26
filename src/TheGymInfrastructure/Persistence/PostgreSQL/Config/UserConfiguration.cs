using TheGymInfrastructure.Persistence.PostgreSQL.Config.Common;
using TheGymDomain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace TheGymInfrastructure.Persistence.PostgreSQL.Config
{
    internal class UserConfiguration : EntityConfiguration<User>
    {
        //TODO: Do I have to specify tablename or will it default to user
        private const string TableName = "User";
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable(TableName);

            builder.Property(x => x.FirstName)
                .HasColumnType("varchar(256)")
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasColumnType("varchar(256)")
                .IsRequired();

            builder.Property(x => x.Phone)
                .IsRequired();
        }

        public class UserValidator : EntityValidator<User>
        {
            public UserValidator()
            {
                RuleFor(x => x.FirstName)
                    .Length(1, 256)
                    .NotEmpty()
                    .WithMessage("First name cannot be empty")
                    .WithErrorCode("First name error");
                RuleFor(x => x.FirstName)
                    .Length(1, 256)
                    .NotEmpty()
                    .WithMessage("First name cannot be empty")
                    .WithErrorCode("First name error");
                RuleFor(x => x.Phone)
                    .Matches(@"\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$")
                    .Length(10, 14)
                    .WithMessage("Phone Format (xxx)-xxx-xxxx, xxx-xxx-xxxx, or xxxxxxxxxx")
                    .NotEmpty()
                    .WithMessage("Phone cannot be empty")
                    .WithErrorCode("Phone error");
            }
        }
    }
}
