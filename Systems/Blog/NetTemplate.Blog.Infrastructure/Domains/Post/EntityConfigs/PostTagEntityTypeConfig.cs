using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTemplate.Blog.ApplicationCore.Post;
using NetTemplate.Blog.ApplicationCore.User;
using NetTemplate.Shared.Infrastructure.Persistence.Extensions;
using CommonConstraintNames = NetTemplate.Shared.Infrastructure.Persistence.Constants.ConstraintNames;
using CommonConstraints = NetTemplate.Shared.ApplicationCore.Common.Constants.Constraints;

namespace NetTemplate.Blog.Infrastructure.Domains.Post.EntityConfigs
{
    class PostTagEntityTypeConfig : IEntityTypeConfiguration<PostTagEntity>
    {
        public void Configure(EntityTypeBuilder<PostTagEntity> builder)
        {
            builder.Property(e => e.Value)
                .HasMaxLength(CommonConstraints.MaxStringLength)
                .IsRequired();

            builder.HasOne<PostEntity>()
                .WithMany(e => e.Tags)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasDefaultConstraintName(postfix: CommonConstraintNames.NoRestrictForeignKeyConstraintPostfix);

            builder.HasOne<UserPartialEntity>()
                .WithMany()
                .HasForeignKey(e => e.CreatorId);
        }
    }
}
