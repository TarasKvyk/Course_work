﻿// <auto-generated />
using BookStore.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231027213640_AddedBooksandAuthorsTables")]
    partial class AddedBooksandAuthorsTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookStore.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Country = "Ukraine",
                            Name = "Taras",
                            Surname = "Author"
                        });
                });

            modelBuilder.Entity("BookStore.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Languqge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 1,
                            CategoryId = 1,
                            Description = "Description",
                            Languqge = "Ukrainian",
                            Price = 200.0,
                            Title = "Test book",
                            Year = 2023
                        });
                });

            modelBuilder.Entity("BookStore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryDescrition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KeyWords")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("BookStore.Models.ChildrenCategory", b =>
                {
                    b.HasBaseType("BookStore.Models.Category");

                    b.Property<int>("PurposeAge")
                        .HasColumnType("int");

                    b.ToTable("ChildrenBooks", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 3,
                            CategoryDescrition = "Child desc",
                            IconUrl = "",
                            KeyWords = "Fairy tale",
                            Name = "Children's literature",
                            PurposeAge = 10
                        });
                });

            modelBuilder.Entity("BookStore.Models.DictionaryCategory", b =>
                {
                    b.HasBaseType("BookStore.Models.Category");

                    b.Property<string>("IntoLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NativeLanguage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("DictionaryBooks", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.FictionCategory", b =>
                {
                    b.HasBaseType("BookStore.Models.Category");

                    b.Property<string>("LiteraryFormat")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("FictionBook", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.HistoryCategory", b =>
                {
                    b.HasBaseType("BookStore.Models.Category");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("HistoryBooks", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryDescrition = "HISTORY desc",
                            IconUrl = "",
                            KeyWords = "Ukraine history",
                            Name = "History",
                            Period = "19"
                        },
                        new
                        {
                            Id = 2,
                            CategoryDescrition = "HISTORY desc",
                            IconUrl = "",
                            KeyWords = "USA history",
                            Name = "History",
                            Period = "19"
                        });
                });

            modelBuilder.Entity("BookStore.Models.ScientificCategory", b =>
                {
                    b.HasBaseType("BookStore.Models.Category");

                    b.Property<string>("KnowledgeBranch")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("ScientificBook", (string)null);
                });

            modelBuilder.Entity("BookStore.Models.Book", b =>
                {
                    b.HasOne("BookStore.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookStore.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BookStore.Models.ChildrenCategory", b =>
                {
                    b.HasOne("BookStore.Models.Category", null)
                        .WithOne()
                        .HasForeignKey("BookStore.Models.ChildrenCategory", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookStore.Models.DictionaryCategory", b =>
                {
                    b.HasOne("BookStore.Models.Category", null)
                        .WithOne()
                        .HasForeignKey("BookStore.Models.DictionaryCategory", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookStore.Models.FictionCategory", b =>
                {
                    b.HasOne("BookStore.Models.Category", null)
                        .WithOne()
                        .HasForeignKey("BookStore.Models.FictionCategory", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookStore.Models.HistoryCategory", b =>
                {
                    b.HasOne("BookStore.Models.Category", null)
                        .WithOne()
                        .HasForeignKey("BookStore.Models.HistoryCategory", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookStore.Models.ScientificCategory", b =>
                {
                    b.HasOne("BookStore.Models.Category", null)
                        .WithOne()
                        .HasForeignKey("BookStore.Models.ScientificCategory", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
