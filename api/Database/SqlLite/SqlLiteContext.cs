using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MockResponse{
    public class SqlLiteContext : DbContext {
        public DbSet<Response> Responses { get; set; }

        public DbSet<Domain> Domains { get; set; }

        public DbSet<Header> Headers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./MockResponse.db");
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder
                .Entity<Domain>()
                .HasMany(d => d.Responses)
                .WithOne(r => r.Domain);
            
            builder
                .Entity<Response>()
                .HasMany(r => r.Headers)
                .WithOne(h => h.Response)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Entity<Header>()
                .HasOne<Response>();
        }
    }
}