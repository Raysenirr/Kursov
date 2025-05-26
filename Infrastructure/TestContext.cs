using Education.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Infrastructure
{
    public class TestContext : DbContext
    {
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Grade>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.Property(x => x.LessonId).IsRequired();
                builder.HasOne<Lesson>()
                       .WithMany("_grades")
                       .HasForeignKey(x => x.LessonId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Lesson>(builder =>
            {
                builder.HasKey(x => x.Id);
                builder.HasMany<Grade>("_grades")
                       .WithOne()
                       .HasForeignKey(x => x.LessonId)
                       .OnDelete(DeleteBehavior.Cascade);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=Education1;Username=postgres;Password=123");
        }
    }

}
