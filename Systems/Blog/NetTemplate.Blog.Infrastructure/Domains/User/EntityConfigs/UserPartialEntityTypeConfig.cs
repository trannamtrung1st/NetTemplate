using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Blog.Infrastructure.Domains.User.EntityConfigs
{
    class UserPartialEntityTypeConfig : IEntityTypeConfiguration<UserPartialEntity>
    {
        public void Configure(EntityTypeBuilder<UserPartialEntity> builder)
        {
            builder.Property(e => e.UserCode)
                .HasMaxLength(AppEntity.Constraints.MaxStringLength)
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasMaxLength(AppEntity.Constraints.MaxStringLength)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(AppEntity.Constraints.MaxStringLength)
                .IsRequired();
        }
    }
}
