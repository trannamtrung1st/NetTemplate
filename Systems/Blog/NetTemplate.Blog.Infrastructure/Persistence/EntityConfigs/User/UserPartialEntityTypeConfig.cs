using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTemplate.Blog.ApplicationCore.User;
using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Blog.Infrastructure.Persistence.EntityConfigs.User
{
    class UserPartialEntityTypeConfig : IEntityTypeConfiguration<UserPartialEntity>
    {
        public void Configure(EntityTypeBuilder<UserPartialEntity> builder)
        {
            builder.Property(e => e.UserCode)
                .HasMaxLength(CommonConstraints.MaxStringLength)
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasMaxLength(CommonConstraints.MaxStringLength)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(CommonConstraints.MaxStringLength)
                .IsRequired();

            builder.Ignore(e => e.FullName);
        }
    }
}
