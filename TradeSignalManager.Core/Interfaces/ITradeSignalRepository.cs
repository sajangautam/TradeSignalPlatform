using TradeSignalManager.Core.Entities;

namespace TradeSignalManager.Core.Interfaces
{
    public interface ITradeSignalRepository
    {
        Task<TradeSignal> AddAsync(TradeSignal signal);
        Task<IEnumerable<TradeSignal>> GetAllAsync();
        Task<TradeSignal?> GetByIdAsync(int id);
    }
}
