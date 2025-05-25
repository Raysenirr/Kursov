using System;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                   .HasConversion(name => name.Value, value => new PersonName(value))
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasOne(x => x.Group)
                   .WithMany(x => x.Students)
                   .IsRequired();

            builder.Navigation(x => x.Group).AutoInclude();

            builder.HasMany<Lesson>("_lessons")
                   .WithMany()
                   .UsingEntity(j => j.ToTable("StudentLessons"));

            var navLessons = builder.Metadata.FindNavigation("_lessons");
            if (navLessons is not null)
                navLessons.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany<Grade>("_grades")
                   .WithOne(g => g.Student)
                   .HasForeignKey("StudentId")
                   .OnDelete(DeleteBehavior.Cascade);

            var gradesNavigation = builder.Metadata.FindNavigation("_grades");
            if (gradesNavigation != null)
                gradesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.Ignore(x => x.AttendedLessons);
            builder.Ignore(x => x.RecievedGrades);
        }
    }

}
