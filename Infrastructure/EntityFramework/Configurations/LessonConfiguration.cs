using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        // Ключ
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Время урока и статус
        builder.Property(x => x.ClassTime).IsRequired();
        builder.Property(x => x.State).IsRequired();

        // Тема урока — value object
        builder.Property(x => x.Topic)
               .IsRequired()
               .HasMaxLength(100)
               .HasConversion(
                   topic => topic.Value,
                   value => new LessonTopic(value)
               );

        // Навигации
        builder.HasOne(x => x.Group)
               .WithMany()
               .IsRequired();

        builder.HasOne(x => x.Teacher)
               .WithMany("_lessons")
               .IsRequired();

        // ❗ Игнорировать вычисляемое свойство AssignedHomeworks
        builder.Ignore(x => x.AssignedHomeworks);

        // Домашние задания — основное навигационное свойство
        builder.HasMany(x => x.Homeworks)
               .WithOne(h => h.Lesson)
               .HasForeignKey("LessonId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Homeworks)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}


