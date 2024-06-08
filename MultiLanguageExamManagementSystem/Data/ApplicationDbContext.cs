using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<TakenExam> TakenExams { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<LocalizationResource> LocalizationResources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Language>()
                .HasMany<LocalizationResource>()
                .WithOne(lr => lr.Language)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Language>()
                .HasIndex(l => new { l.CountryId, l.LanguageCode })
                .IsUnique();

            modelBuilder.Entity<Country>()
                .HasMany<Language>()
                .WithOne(l => l.Country)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LocalizationResource>()
                .HasIndex(lr => new { lr.LanguageId, lr.Namespace, lr.Key })
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Exams)
                .WithOne(e => e.Professor)
                .HasForeignKey(e => e.ProfessorId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.TakenExams)
                .WithOne(te => te.User)
                .HasForeignKey(te => te.UserId);

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.Questions)
                .WithMany(q => q.Exams)
                .UsingEntity(j => j.ToTable("ExamQuestions"));

            modelBuilder.Entity<Exam>()
                .HasMany(e => e.TakenExams)
                .WithOne(te => te.Exam)
                .HasForeignKey(te => te.ExamId);

            modelBuilder.Entity<TakenExam>()
                .HasOne(te => te.Exam)
                .WithMany(e => e.TakenExams)
                .HasForeignKey(te => te.ExamId);
        }

    }
}
