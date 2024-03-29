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
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<OrderHeader> OrderHeaders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)  
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.SetNull); 

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany()
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.SetNull); 

            // Налаштування таблиці для базового класу Category
            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.Id);

            // Налаштування таблиці для підкласу HistoryCategory з наслідуванням
            modelBuilder.Entity<HistoryCategory>()
                .ToTable("HistoryBooks")
                .HasBaseType<Category>();

            modelBuilder.Entity<HistoryCategory>()
                .Property(h => h.Period);

            // Налаштування таблиці для підкласу ChildrenCategory з наслідуванням
            modelBuilder.Entity<ChildrenCategory>()
                .ToTable("ChildrenBooks")
                .HasBaseType<Category>();

            modelBuilder.Entity<ChildrenCategory>()
                .Property(h => h.PurposeAge);

            // Налаштування таблиці для підкласу DictionaryCategory з наслідуванням
            modelBuilder.Entity<DictionaryCategory>()
                .ToTable("DictionaryBooks")
                .HasBaseType<Category>();
            modelBuilder.Entity<DictionaryCategory>()
                .Property(d => d.NativeLanguage); 
            modelBuilder.Entity<DictionaryCategory>()
                .Property(d => d.IntoLanguage);

            // Налаштування таблиці для підкласу FictionCategory з наслідуванням
            modelBuilder.Entity<FictionCategory>()
                .ToTable("FictionBook")
                .HasBaseType<Category>();

            modelBuilder.Entity<FictionCategory>()
                .Property(h => h.LiteraryFormat);

            // Налаштування таблиці для підкласу ScientificCategory з наслідуванням
            modelBuilder.Entity<ScientificCategory>()
                .ToTable("ScientificBook")
                .HasBaseType<Category>();

            modelBuilder.Entity<ScientificCategory>()
                .Property(h => h.KnowledgeBranch);
        }
    }
}
