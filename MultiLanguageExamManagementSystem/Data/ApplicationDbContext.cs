using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Models.Entities;
using System.Collections.Generic;

namespace MultiLanguageExamManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<Language> Languages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LocalizationResource> LocalizationResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>()
                .HasMany<Language>(c => c.Languages)
                .WithOne(l => l.Country)
                .HasForeignKey(l => l.CountryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Language>()
                .HasMany<LocalizationResource>(l => l.LocalizationResources)
                .WithOne(lr => lr.Language)
                .HasForeignKey(lr => lr.LanguageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}