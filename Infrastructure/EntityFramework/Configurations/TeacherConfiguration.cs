using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        // Ключ и ID
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        // Имя преподавателя
        builder.Property(x => x.Name)
               .HasConversion(
                   name => name.Value,
                   value => new PersonName(value)
               )
               .IsRequired()
               .HasMaxLength(50);

        // Навигация к приватным полям _lessons и _grades
        var lessonsNav = builder.Metadata.FindNavigation("_lessons");
        if (lessonsNav is not null)
            lessonsNav.SetPropertyAccessMode(PropertyAccessMode.Field);

        var gradesNav = builder.Metadata.FindNavigation("_grades");
        if (gradesNav is not null)
            gradesNav.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Owned-тип: HomeworkBank
        builder.OwnsOne(x => x.HomeworkBank, bank =>
        {
            // ❗ НЕ используем bank.Navigation("_templates") — EF не считает это навигацией

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

