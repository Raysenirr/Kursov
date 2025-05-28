using Education.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
            // Устанавливаем первичный ключ по Id
            builder.HasKey(x => x.Id);

            // Указываем, что Id создаётся автоматически при добавлении
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            // Свойство Mark (оценка) обязательно для заполнения
            builder.Property(x => x.Mark).IsRequired();

            // Конвертация времени оценки к UTC — и при сохранении, и при чтении
            builder.Property(x => x.GradedTime)
        .IsRequired()
        .HasConversion(
        src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
        dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
        );

            // Связь с сущностью Student (один ко многим, навигация через поле _grades)
            builder.HasOne(x => x.Student)
        .WithMany("_grades")
        .HasForeignKey(x => x.StudentId)
        .OnDelete(DeleteBehavior.Cascade) // удаление студента удаляет и оценки
                   .IsRequired();

            // Связь с Lesson (один ко многим, поле _grades, без явной навигации)
            builder.HasOne<Lesson>()
        .WithMany("_grades")
        .HasForeignKey(x => x.LessonId)
        .OnDelete(DeleteBehavior.Cascade); // удаление урока удаляет оценки

            // Связь с Teacher (один ко многим, поле _grades), но с ограничением на удаление
            builder.HasOne(x => x.Teacher)
        .WithMany("_grades")
        .HasForeignKey(x => x.TeacherId)
        .OnDelete(DeleteBehavior.Restrict) // запрет на удаление, если есть оценки
                   .IsRequired();

            // Навигация к Lesson через private field (чтение через поле, а не через свойство)
            builder.Navigation(nameof(Grade.Lesson)).UsePropertyAccessMode(PropertyAccessMode.Field);

            // Автоматически загружать связанную сущность Student при запросе оценки
            builder.Navigation(x => x.Student).AutoInclude();

            // Автоматически загружать связанную сущность Teacher при запросе оценки
            builder.Navigation(x => x.Teacher).AutoInclude();

            // Ограничение: нельзя выставить более одной оценки студенту за один урок
            builder.HasIndex(x => new { x.StudentId, x.LessonId }).IsUnique();
        }
    }


