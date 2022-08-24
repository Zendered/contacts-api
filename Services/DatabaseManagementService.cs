using ContactsApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactsApi.Services
{
    public static class DatabaseManagementService
    {
        public static void MigrationInitialisation(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var serviceDb = serviceScope.ServiceProvider.GetService<ContactsContext>();

                serviceDb.Database.Migrate();
            }
        }
    }
}