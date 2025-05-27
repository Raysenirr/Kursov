
using Education.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Education.Infrastructure.EntityFramework;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<Grade> Grades { get; set; }

    public DbSet<Homework> Homeworks { get; set; }
    public DbSet<HomeworkSubmission> HomeworkSubmissions { get; set; }

    // НЕ добавляй SubmittedHomeworkInfo — он не маппится на таблицу
    // public DbSet<SubmittedHomeworkInfo> SubmittedHomeworkInfos { get; set; } // ❌

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(); // для отладки
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Подключает все IEntityTypeConfiguration<T> из текущей сборки
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var navigation in entity.GetNavigations())
            {
                if (navigation.PropertyInfo == null && navigation.FieldInfo != null)
                {
                    navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
                }
            }
        }
        }
}

