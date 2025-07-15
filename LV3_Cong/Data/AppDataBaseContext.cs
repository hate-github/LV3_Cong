using LV3_Cong.Models;
using Microsoft.EntityFrameworkCore;

namespace LV3_Cong.Data
{
    public class AppDataBaseContext : DbContext
    {
        public AppDataBaseContext(DbContextOptions<AppDataBaseContext> options)
            : base(options)
        { }

        public DbSet<Birthday> Birthdays { get; set; }
    }
}
