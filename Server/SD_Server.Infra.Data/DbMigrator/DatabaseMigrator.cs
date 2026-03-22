using Microsoft.EntityFrameworkCore;
using SD_Server.Infra.Data.Context;

namespace SD_Server.Infra.Data.DbMigrator
{
    public class DatabaseMigrator (SdServerDbContext context)
    {
        public async Task MigrateAsync()
        {
            var db = context.Database;
            if (!db.IsInMemory() && db.GetPendingMigrations().Any())
                await db.MigrateAsync();
        }
    }
}
