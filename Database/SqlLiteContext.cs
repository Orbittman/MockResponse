using Microsoft.EntityFrameworkCore;

namespace MockResponse{
    public class SqlLiteContext : DbContext {
        public DbSet<Response> Responses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./MockResponse.db");
        }
    }
}