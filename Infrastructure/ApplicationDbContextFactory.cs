using Education.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Education.Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Пример строки подключения — замени на свою при необходимости
        optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=Education2;UserID=postgres;Password=123;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
