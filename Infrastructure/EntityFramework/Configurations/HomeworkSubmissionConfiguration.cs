using Education.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class HomeworkSubmissionConfiguration : IEntityTypeConfiguration<HomeworkSubmission>
{
    public void Configure(EntityTypeBuilder<HomeworkSubmission> builder)
    {
        // Композитный ключ (StudentId + HomeworkId)
        builder.HasKey(x => new { x.StudentId, x.HomeworkId });

        // Дата отправки
        builder.Property(x => x.SubmissionDate)
               .IsRequired();

        // Навигация к студенту
        builder.HasOne(x => x.Student)
               .WithMany()
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        // Навигация к Homework — ❗ БЕЗ указания Submissions, чтобы не конфликтовало с _submissions
        builder.HasOne(x => x.Homework)
               .WithMany() // 👈 важно: без .WithMany(h => h.Submissions)
               .HasForeignKey(x => x.HomeworkId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        // Автоподключение студента (если нужно)
        builder.Navigation(x => x.Student).AutoInclude();
    }
}


