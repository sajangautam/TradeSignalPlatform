using Microsoft.EntityFrameworkCore;
using TradeSignalManager.Core.Entities;

namespace TradeSignalManager.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TradeSignal> TradeSignals { get; set; }
    }
}
