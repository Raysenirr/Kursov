using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.ClassTime).IsRequired();
        builder.Property(x => x.State).IsRequired().HasConversion<string>();

        builder.Property(x => x.Topic)
               .IsRequired()
               .HasMaxLength(100)
               .HasConversion(
                   topic => topic.Value,
                   value => new LessonTopic(value));

        builder.HasOne(x => x.Group)
               .WithMany()
               .HasForeignKey("GroupId")
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasOne(x => x.Teacher)
               .WithMany("_lessons")
               .HasForeignKey(x => x.TeacherId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.HasMany<Grade>("_grades")
       .WithOne("Lesson")
       .HasForeignKey("LessonId")
       .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<Homework>("_homeworks")
               .WithOne("Lesson")
               .HasForeignKey("LessonId")
               .OnDelete(DeleteBehavior.Cascade);


        builder.Metadata.FindNavigation("_grades")!.SetPropertyAccessMode(PropertyAccessMode.Field);
        builder.Metadata.FindNavigation("_homeworks")!.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Metadata.FindNavigation("_homeworks")!
               .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(x => x.Homeworks);
        builder.Ignore(x => x.AssignedGrades);
        builder.Ignore(x => x.AssignedHomeworks);

    }
}


