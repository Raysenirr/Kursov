using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        // Первичный ключ
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Конфигурация value object: PersonName
        builder.Property(x => x.Name)
               .HasConversion(n => n.Value, v => new PersonName(v))
               .IsRequired()
               .HasMaxLength(50);

        // Связь с Lesson (через _lessons)
        builder.HasMany<Lesson>("_lessons")
               .WithOne(l => l.Teacher)
               .HasForeignKey(l => l.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);

        // Связь с Grade (через _grades)
        builder.HasMany<Grade>("_grades")
               .WithOne(g => g.Teacher)
               .HasForeignKey(g => g.TeacherId)
               .OnDelete(DeleteBehavior.Restrict);

        // Доступ к приватным полям
        builder.Metadata.FindNavigation("_lessons")!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation("_grades")!.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Игнорируем вычисляемые коллекции
        builder.Ignore(t => t.TeachedLessons);
        builder.Ignore(t => t.ScheduledLessons);
        builder.Ignore(t => t.AssignedGrades);

        // Настройка владения HomeworkBank → HomeworkTemplate (owned collection)
        builder.OwnsOne(x => x.HomeworkBank, bank =>
        {
            bank.OwnsMany<HomeworkTemplate>("_templates", template =>
            {
                template.Property(t => t.Topic)
                        .HasConversion(t => t.Value, v => new LessonTopic(v))
                        .HasMaxLength(100)
                        .IsRequired();

                template.Property(t => t.Title)
                        .HasConversion(t => t.Value, v => new HomeworkTitle(v))
                        .HasMaxLength(100)
                        .IsRequired();

                template.WithOwner().HasForeignKey("TeacherId");
                template.HasKey("TeacherId", "Topic");
            });
        });
    }
}