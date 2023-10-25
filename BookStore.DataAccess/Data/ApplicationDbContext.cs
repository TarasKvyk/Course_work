using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HistoryBook>().HasData(
                new HistoryBook
                {
                    Id = 1,
                    Period = "19",
                    Name = "History",
                    KeyWords = "Ukraine history",
                    CategoryDescrition = "HISTORY desc",
                    IconUrl = ""
                });


            modelBuilder.Entity<HistoryBook>().HasData(
                new HistoryBook
                {
                    Id = 2,
                    Period = "19",
                    Name = "History",
                    KeyWords = "USA history",
                    CategoryDescrition = "HISTORY desc",
                    IconUrl = ""
                });

            modelBuilder.Entity<ChildrenBook>().HasData(
                new ChildrenBook
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
            modelBuilder.Entity<HistoryBook>()
                .ToTable("HistoryBooks")
                .HasBaseType<Category>();

            // Налаштування властивостей для підкласу Historical
            modelBuilder.Entity<HistoryBook>()
                .Property(h => h.Period);



            modelBuilder.Entity<ChildrenBook>()
                .ToTable("ChildrenBooks")
                .HasBaseType<Category>();

            modelBuilder.Entity<ChildrenBook>()
                .Property(h => h.PurposeAge);



            modelBuilder.Entity<DictionaryBook>()
                .ToTable("DictionaryBooks")
                .HasBaseType<Category>();
            modelBuilder.Entity<DictionaryBook>()
                .Property(d => d.NativeLanguage); 
            modelBuilder.Entity<DictionaryBook>()
                .Property(d => d.IntoLanguage); 


            modelBuilder.Entity<FictionBook>()
                .ToTable("FictionBook")
                .HasBaseType<Category>();

            modelBuilder.Entity<FictionBook>()
                .Property(h => h.LiteraryFormat);



            modelBuilder.Entity<ScientificBook>()
                .ToTable("ScientificBook")
                .HasBaseType<Category>();

            modelBuilder.Entity<ScientificBook>()
                .Property(h => h.KnowledgeBranch);
        }
    }
}
