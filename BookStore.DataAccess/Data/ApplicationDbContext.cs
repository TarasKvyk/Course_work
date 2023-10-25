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

            modelBuilder.Entity<Historical>().HasData(
                new Historical
                {
                    Id = 1,
                    Descrition = "History of Ukraine",
                    Period = "19",
                    KeyWords = "Ukraine history",
                    IconUrl = ""
                });


            modelBuilder.Entity<Historical>().HasData(
                new Historical
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
            modelBuilder.Entity<Historical>()
                .ToTable("HistoricalCategories")
                .HasBaseType<Category>();

            // Налаштування властивостей для підкласу Historical
            modelBuilder.Entity<Historical>()
                .Property(h => h.Period);

            // Додайте інші налаштування для інших підкласів
        }
    }
}
