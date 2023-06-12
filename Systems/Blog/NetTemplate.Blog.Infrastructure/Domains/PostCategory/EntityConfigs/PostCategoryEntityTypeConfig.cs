using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTemplate.Blog.ApplicationCore.PostCategory;
using NetTemplate.Shared.ApplicationCore.Common.Entities;

namespace NetTemplate.Blog.Infrastructure.Domains.PostCategory.EntityConfigs
{
    class PostCategoryEntityTypeConfig : IEntityTypeConfiguration<PostCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<PostCategoryEntity> builder)
        {
            builder.Property(e => e.Name)
                .HasMaxLength(AppEntity.Constraints.MaxStringLength)
                .IsRequired();

            builder.HasIndex(e => e.Name)
                .IsUnique();

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId)
                .OnDelete(DeleteBehavior.Restrict); // [OPTIONAL] since we restricted globally in OnModelCreating
        }
    }
}
