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
                    Descrition = "History of Ukraine",
                    Period = "19",
                    KeyWords = "Ukraine history",
                    IconUrl = ""
                });


            modelBuilder.Entity<HistoryBook>().HasData(
                new HistoryBook
                {
                    Id = 3,
                    Descrition = "History of USA",
                    Period = "19",
                    KeyWords = "USA history",
                    IconUrl = ""
                });

            // Налаштування таблиці для базового класу Category
            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.Id);

            // Налаштування таблиці для підкласу Historical з наслідуванням
            modelBuilder.Entity<HistoryBook>()
                .ToTable("HistoricalCategories")
                .HasBaseType<Category>();

            // Налаштування властивостей для підкласу Historical
            modelBuilder.Entity<HistoryBook>()
                .Property(h => h.Period);

            // Додайте інші налаштування для інших підкласів
        }
    }
}
