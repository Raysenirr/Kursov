using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Education.Infrastructure.EntityFramework;

class TestDbContextStartup
{
    static void Main(string[] args)
    {
        try
        {
            var factory = new ApplicationDbContextFactory();
            using var context = factory.CreateDbContext(args);

            Console.WriteLine("✅ DbContext создан успешно.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Ошибка при создании DbContext:");
            Console.WriteLine(ex.ToString());
        }
    }
}
