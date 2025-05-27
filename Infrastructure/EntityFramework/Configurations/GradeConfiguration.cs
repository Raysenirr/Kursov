using Education.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Education.Infrastructure.EntityFramework.Configurations;

public class GradeConfiguration : IEntityTypeConfiguration<Grade>
{
    public void Configure(EntityTypeBuilder<Grade> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.Mark).IsRequired();

        builder.Property(x => x.GradedTime)
               .IsRequired()
               .HasConversion(
                   src => src.Kind == DateTimeKind.Utc ? src : DateTime.SpecifyKind(src, DateTimeKind.Utc),
                   dst => dst.Kind == DateTimeKind.Utc ? dst : DateTime.SpecifyKind(dst, DateTimeKind.Utc)
               );

        builder.HasOne(x => x.Student)
               .WithMany("_grades") // к private _grades в Student
               .HasForeignKey(x => x.StudentId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();

        builder.HasOne<Lesson>() // 👈 без лямбды!
       .WithMany("_grades")
       .HasForeignKey(x => x.LessonId)
       .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.Teacher)
               .WithMany("_grades") // к private _grades в Teacher
               .HasForeignKey(x => x.TeacherId)
               .OnDelete(DeleteBehavior.Restrict)
               .IsRequired();

        builder.Navigation(nameof(Grade.Lesson)).UsePropertyAccessMode(PropertyAccessMode.Field);


        builder.Navigation(x => x.Student).AutoInclude();
        builder.Navigation(x => x.Teacher).AutoInclude();

        builder.HasIndex(x => new { x.StudentId, x.LessonId }).IsUnique();
    }
}


