using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
{
    public void Configure(EntityTypeBuilder<Homework> builder)
    {
        // Первичный ключ
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).ValueGeneratedOnAdd();

        // Конфигурация value object HomeworkTitle
        builder.Property(h => h.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasConversion(
                title => title.Value,
                value => new HomeworkTitle(value)
            );

        // Связь с Lesson (один ко многим)
        builder.HasOne(h => h.Lesson)
               .WithMany()
               .HasForeignKey(h => h.LessonId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        // Связь с HomeworkSubmission через приватное поле _submissions
        builder.HasMany<HomeworkSubmission>("_submissions")
               .WithOne(s => s.Homework)
               .HasForeignKey("HomeworkId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        // Устанавливаем доступ к _submissions через поле
        var nav = builder.Metadata.FindNavigation("_submissions");
        if (nav is not null)
            nav.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}


