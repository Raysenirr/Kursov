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
            // Первичный ключ
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            // Конфигурация value object: PersonName
            builder.Property(x => x.Name)
                   .HasConversion(name => name.Value, value => new PersonName(value))
                   .IsRequired()
                   .HasMaxLength(50);

            // Связь с группой
            builder.HasOne(x => x.Group)
                   .WithMany(x => x.Students)
                   .IsRequired();

            builder.Navigation(x => x.Group).AutoInclude();

            // Связь many-to-many с Lesson через приватное поле _lessons
            builder.HasMany<Lesson>("_lessons")
                   .WithMany()
                   .UsingEntity(j => j.ToTable("StudentLessons"));

            builder.Metadata.FindNavigation("_lessons")?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Связь one-to-many с Grade через _grades
            builder.HasMany<Grade>("_grades")
                   .WithOne(g => g.Student)
                   .HasForeignKey("StudentId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation("_grades")?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Игнорируем вычисляемые свойства
            builder.Ignore(x => x.AttendedLessons);
            builder.Ignore(x => x.RecievedGrades);
        }
    }
}