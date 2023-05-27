﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTemplate.Blog.Infrastructure.Persistence;

#nullable disable

namespace NetTemplate.Blog.Infrastructure.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.Comment.CommentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("DeletorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("LastModifyUserId")
                        .HasColumnType("int");

                    b.Property<int>("OnPostId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("OnPostId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.Post.PostEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("DeletorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("LastModifyUserId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.Post.PostTagEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("LastModifyUserId")
                        .HasColumnType("int");

                    b.Property<int?>("PostEntityId")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PostEntityId");

                    b.ToTable("PostTag");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.PostCategory.PostCategoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("DeletorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("LastModifyUserId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.User.UserPartialEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("CreatedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("DeletedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("DeletorId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LastModifiedTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int?>("LastModifyUserId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserPartial");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.Comment.CommentEntity", b =>
                {
                    b.HasOne("NetTemplate.Blog.ApplicationCore.User.UserPartialEntity", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("NetTemplate.Blog.ApplicationCore.Post.PostEntity", "OnPost")
                        .WithMany()
                        .HasForeignKey("OnPostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("OnPost");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.Post.PostEntity", b =>
                {
                    b.HasOne("NetTemplate.Blog.ApplicationCore.PostCategory.PostCategoryEntity", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NetTemplate.Blog.ApplicationCore.User.UserPartialEntity", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Category");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.Post.PostTagEntity", b =>
                {
                    b.HasOne("NetTemplate.Blog.ApplicationCore.Post.PostEntity", null)
                        .WithMany("Tags")
                        .HasForeignKey("PostEntityId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.PostCategory.PostCategoryEntity", b =>
                {
                    b.HasOne("NetTemplate.Blog.ApplicationCore.User.UserPartialEntity", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.Post.PostEntity", b =>
                {
                    b.Navigation("Tags");
                });

            modelBuilder.Entity("NetTemplate.Blog.ApplicationCore.PostCategory.PostCategoryEntity", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
