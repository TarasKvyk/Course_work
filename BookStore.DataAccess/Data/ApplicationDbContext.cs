﻿using Microsoft.EntityFrameworkCore;
using System;
using BookStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>().HasData(
                new Book { 
                    Id = 1, 
                    Title = "Test book", 
                    AuthorId = 1,
                    CategoryId = 1,
                    Year = 2023,
                    Languqge = "Ukrainian",
                    Description = "Description",
                    Price = 200.0
                });


            modelBuilder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "Taras",
                    Surname = "Author",
                    Country = "Ukraine",
                });

            modelBuilder.Entity<HistoryCategory>().HasData(
                new HistoryCategory
                {
                    Id = 1,
                    Period = "19",
                    Name = "History",
                    KeyWords = "Ukraine history",
                    CategoryDescrition = "HISTORY desc",
                    IconUrl = ""
                });


            modelBuilder.Entity<HistoryCategory>().HasData(
                new HistoryCategory
                {
                    Id = 2,
                    Period = "19",
                    Name = "History",
                    KeyWords = "USA history",
                    CategoryDescrition = "HISTORY desc",
                    IconUrl = ""
                });

            modelBuilder.Entity<ChildrenCategory>().HasData(
                new ChildrenCategory
                {
                    Id = 3,
                    PurposeAge = 10,
                    Name = "Children's literature",
                    CategoryDescrition = "Child desc",
                    KeyWords = "Fairy tale",
                    IconUrl = ""
                });


            // Налаштування таблиці для базового класу Category
            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.Id);



            // Налаштування таблиці для підкласу Historical з наслідуванням
            modelBuilder.Entity<HistoryCategory>()
                .ToTable("HistoryBooks")
                .HasBaseType<Category>();

            // Налаштування властивостей для підкласу Historical
            modelBuilder.Entity<HistoryCategory>()
                .Property(h => h.Period);



            modelBuilder.Entity<ChildrenCategory>()
                .ToTable("ChildrenBooks")
                .HasBaseType<Category>();

            modelBuilder.Entity<ChildrenCategory>()
                .Property(h => h.PurposeAge);



            modelBuilder.Entity<DictionaryCategory>()
                .ToTable("DictionaryBooks")
                .HasBaseType<Category>();
            modelBuilder.Entity<DictionaryCategory>()
                .Property(d => d.NativeLanguage); 
            modelBuilder.Entity<DictionaryCategory>()
                .Property(d => d.IntoLanguage); 


            modelBuilder.Entity<FictionCategory>()
                .ToTable("FictionBook")
                .HasBaseType<Category>();

            modelBuilder.Entity<FictionCategory>()
                .Property(h => h.LiteraryFormat);



            modelBuilder.Entity<ScientificCategory>()
                .ToTable("ScientificBook")
                .HasBaseType<Category>();

            modelBuilder.Entity<ScientificCategory>()
                .Property(h => h.KnowledgeBranch);
        }
    }
}
