using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTemplate.Blog.ApplicationCore.Post;
using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.EntityConfigs
{
    class PostEntityTypeConfig : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.Property(e => e.Title)
                .HasMaxLength(CommonConstraints.MaxStringLength)
                .IsRequired();

            builder.HasIndex(e => e.Title)
                .IsUnique();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.HasOne(e => e.Category)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.CategoryId);

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId);
        }
    }
}
