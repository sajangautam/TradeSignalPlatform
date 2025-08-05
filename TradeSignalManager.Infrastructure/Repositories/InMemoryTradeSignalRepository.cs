using TradeSignalManager.Core.Entities;
using TradeSignalManager.Core.Interfaces;

namespace TradeSignalManager.Infrastructure.Repositories
{
    public class InMemoryTradeSignalRepository : ITradeSignalRepository
    {
        private static readonly List<TradeSignal> _signals = new();
        private readonly object _lock = new();

        public Task<TradeSignal> AddAsync(TradeSignal signal)
        {
            lock (_lock)
            {
                signal.Id = _signals.Count + 1;
                _signals.Add(signal);
                return Task.FromResult(signal);
            }
        }

        public Task<IEnumerable<TradeSignal>> GetAllAsync()
            => Task.FromResult(_signals.AsEnumerable());

        public Task<TradeSignal?> GetByIdAsync(int id)
            => Task.FromResult(_signals.FirstOrDefault(s => s.Id == id));
    }
}
