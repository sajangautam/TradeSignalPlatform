using System.Text.Json;
using TradeSignalManager.Core.Entities;
using TradeSignalManager.Infrastructure.Repositories; // Namespace where your DbContext is

namespace TradeSignalManager.Infrastructure.Data
{
    public static class SeedSp500
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Tickers.Any())
            {
                // Clear existing data just in case
                context.Tickers.RemoveRange(context.Tickers);
                context.SaveChanges();

                var jsonFilePath = Path.Combine(AppContext.BaseDirectory, "TradeSignalManager.Infrastructure", "Data", "sp500.json");
                var jsonData = File.ReadAllText(jsonFilePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var tickers = JsonSerializer.Deserialize<List<Ticker>>(jsonData, options);

                if (tickers != null)
                {
                    context.Tickers.AddRange(tickers);
                    context.SaveChanges();
                }
            }
        }

    }
}
