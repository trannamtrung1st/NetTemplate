using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTemplate.Blog.ApplicationCore.Comment;

namespace NetTemplate.Blog.Infrastructure.Domains.Comment.EntityConfigs
{
    class CommentEntityTypeConfig : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.Property(e => e.Content)
                .HasMaxLength(CommentEntity.Constraints.MaxContentLength)
                .IsRequired();

            builder.HasOne(e => e.OnPost)
                .WithMany()
                .HasForeignKey(e => e.OnPostId);

            builder.HasOne(e => e.Creator)
                .WithMany()
                .HasForeignKey(e => e.CreatorId);
        }
    }
}
