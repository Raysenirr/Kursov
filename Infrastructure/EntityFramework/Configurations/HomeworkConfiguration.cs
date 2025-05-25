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

        // Навигация к уроку (с привязкой к Lesson.Homeworks)
        builder.HasOne(h => h.Lesson)
            .WithMany(l => l.Homeworks)
            .HasForeignKey("LessonId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(h => h.Lesson).AutoInclude();
        builder.Ignore(h => h.Submissions);

        // Приватное поле
        builder.HasMany<HomeworkSubmission>("_submissions")
            .WithOne(s => s.Homework)
            .HasForeignKey("HomeworkId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Безопасно находим навигацию
        var nav = builder.Metadata.FindNavigation("_submissions");
        if (nav is not null)
            nav.SetPropertyAccessMode(PropertyAccessMode.Field);

    }
}

