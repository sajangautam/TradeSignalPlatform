using Microsoft.EntityFrameworkCore;
using TradeSignalManager.Core.Entities;
using TradeSignalManager.Core.Interfaces;

namespace TradeSignalManager.Infrastructure.Repositories
{
    public class EfCoreTradeSignalRepository : ITradeSignalRepository
    {
        private readonly AppDbContext _context;

        public EfCoreTradeSignalRepository(AppDbContext context)
        {
            _context = context;
        }

        // Return the added entity after saving
        public async Task<TradeSignal> AddAsync(TradeSignal signal)
        {
            var entityEntry = await _context.TradeSignals.AddAsync(signal);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;  // return the added TradeSignal
        }

        // Return IEnumerable<TradeSignal>
        public async Task<IEnumerable<TradeSignal>> GetAllAsync()
        {
            return await _context.TradeSignals.ToListAsync();
        }

        // Implement GetByIdAsync
        public async Task<TradeSignal?> GetByIdAsync(int id)
        {
            return await _context.TradeSignals.FindAsync(id);
        }
    }
}
