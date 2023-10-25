﻿// <auto-generated />
using BookStore.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookStore.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookStore.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descrition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IconUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("KeyWords")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("BookStore.Models.Historical", b =>
                {
                    b.HasBaseType("BookStore.Models.Category");

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("HistoricalCategories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Descrition = "History of Ukraine",
                            IconUrl = "",
                            KeyWords = "Ukraine history",
                            Period = "19"
                        },
                        new
                        {
                            Id = 3,
                            Descrition = "History of USA",
                            IconUrl = "",
                            KeyWords = "USA history",
                            Period = "19"
                        });
                });

            modelBuilder.Entity("BookStore.Models.Historical", b =>
                {
                    b.HasOne("BookStore.Models.Category", null)
                        .WithOne()
                        .HasForeignKey("BookStore.Models.Historical", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
