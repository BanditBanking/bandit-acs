using Bandit.ACS.NpgsqlRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace Bandit.ACS.NpgsqlRepository
{
    public class NpgsqlDbContext : DbContext
    {
        public NpgsqlDbContext(DbContextOptions<NpgsqlDbContext> options) : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; } = null!;
    }
}
