using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
{
    public void Configure(EntityTypeBuilder<Homework> builder)
    {
        // Ключ
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id).ValueGeneratedOnAdd();

        // Value Object: HomeworkTitle
        builder.Property(h => h.Title)
            .IsRequired()
            .HasMaxLength(200)
            .HasConversion(
                title => title.Value,
                value => new HomeworkTitle(value)
            );

        // ✅ Простая навигация к Lesson — без WithMany
        builder.HasOne(h => h.Lesson)
               .WithMany() // ← просто оставляем пустым (основная связь настраивается в LessonConfiguration)
               .HasForeignKey(h => h.LessonId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        // ✅ Настройка навигации к сабмишенам
        builder.HasMany<HomeworkSubmission>("_submissions")
               .WithOne(s => s.Homework)
               .HasForeignKey("HomeworkId")
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        var nav = builder.Metadata.FindNavigation("_submissions");
        if (nav is not null)
            nav.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}



