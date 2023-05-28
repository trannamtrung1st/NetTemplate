using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Blog.Infrastructure.Persistence.EntityConfigs.PostCategory
{
    class PostCategoryEntityTypeConfig : IEntityTypeConfiguration<PostCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<PostCategoryEntity> builder)
        {
            builder.Property(e => e.Name)
                .HasMaxLength(CommonConstraints.MaxStringLength)
                .IsRequired();

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); // [OPTIONAL] since we restricted globally in OnModelCreating
        }
    }
}
