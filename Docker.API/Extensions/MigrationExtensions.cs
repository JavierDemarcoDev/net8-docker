using Context;
using Microsoft.EntityFrameworkCore;

namespace Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using AppDbContext dbContext = scope.ServiceProvider.GetService<AppDbContext>()!;

            dbContext.Database.Migrate();
        }
    }
}
