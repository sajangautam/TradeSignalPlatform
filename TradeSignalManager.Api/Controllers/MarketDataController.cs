using Microsoft.AspNetCore.Mvc;
using TradeSignalManager.Core.Entities;
using TradeSignalManager.Infrastructure.Services;
using TradeSignalManager.Infrastructure;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSignalManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AlphaVantageService _alphaVantageService;

        public MarketDataController(AppDbContext context, AlphaVantageService alphaVantageService)
        {
            _context = context;
            _alphaVantageService = alphaVantageService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                return BadRequest("Symbol is required");

            symbol = symbol.ToUpper();

            // 1. Get basic ticker info from DB
            var ticker = _context.Tickers.FirstOrDefault(t => t.Symbol == symbol);
            if (ticker == null)
                return NotFound("Ticker not found");

            // 2. Call Alpha Vantage APIs
            var dailyData = await _alphaVantageService.GetDailyTimeSeriesAsync(symbol);
            var intradayData = await _alphaVantageService.GetIntradayTimeSeriesAsync(symbol);

            if (dailyData == null || intradayData == null)
                return StatusCode(503, "Failed to fetch data from Alpha Vantage");

            // 3. Parse daily close prices (last 30 days for advanced calculations)
            var dailyCloses = GetClosingPrices(dailyData, "Time Series (Daily)", 30);
            // For momentum as before, use last 5 days
            var momentumCloses = dailyCloses.Take(5).ToList();

            // 4. Parse intraday close prices (last 10 intervals)
            var intradayCloses = GetClosingPrices(intradayData, "Time Series (5min)", 10);

            // 5. Calculate existing momentum & volatility
            var momentum = CalculateMomentum(momentumCloses);
            var volatility = CalculateVolatility(intradayCloses);

            // 6. Calculate advanced indicators (only if enough data)
            double rsi = 0;
            double sma20 = 0;
            List<double> ema12 = new List<double>();
            List<double> macd = new List<double>();
            List<double> macdSignal = new List<double>();
            List<double> middleBand = new List<double>();
            List<double> upperBand = new List<double>();
            List<double> lowerBand = new List<double>();



            if (dailyCloses.Count >= 30)
            {
                rsi = TechnicalIndicators.CalculateRSI(dailyCloses);
                sma20 = TechnicalIndicators.CalculateSMA(dailyCloses, 20);
                ema12 = TechnicalIndicators.CalculateEMA(dailyCloses, 12);
                (macd, macdSignal) = TechnicalIndicators.CalculateMACD(dailyCloses);
                (middleBand, upperBand, lowerBand) = TechnicalIndicators.CalculateBollingerBands(dailyCloses);

            }

            double? latestPrice = null;
            if (intradayCloses.Any())
            {
                latestPrice = intradayCloses[0];
            }
            else if (dailyCloses.Any())
            {
                latestPrice = dailyCloses[0];
            }


            // 7. Prepare response with all data and analysis
            var result = new
            {
                ticker.Symbol,
                ticker.Name,
                ticker.Exchange,
                ticker.Sector,
                CurrentPrice = latestPrice.HasValue ? Math.Round(latestPrice.Value, 2) : (double?)null,
                Momentum = Math.Round(momentum, 2),
                Volatility = Math.Round(volatility, 2),
                RSI = Math.Round(rsi, 2),
                SMA20 = Math.Round(sma20, 2),
                EMA12 = RoundDoubleList(ema12),
                MACD = RoundDoubleList(macd),
                MACDSignal = RoundDoubleList(macdSignal),
                BollingerBandsMiddle = RoundDoubleList(middleBand),
                BollingerBandsUpper = RoundDoubleList(upperBand),
                BollingerBandsLower = RoundDoubleList(lowerBand),
                DailyClosePrices = RoundDoubleList(dailyCloses),
                IntradayClosePrices = RoundDoubleList(intradayCloses)
            };
            return Ok(result);
        }

        // Helper: Extract closing prices from JsonDocument, returns newest first
        private List<double> GetClosingPrices(JsonDocument jsonDoc, string seriesKey, int maxCount)
        {
            var closes = new List<double>();
            if (jsonDoc.RootElement.TryGetProperty(seriesKey, out var timeSeries))
            {
                foreach (var property in timeSeries.EnumerateObject())
                {
                    if (property.Value.TryGetProperty("4. close", out var closeValue))
                    {
                        if (double.TryParse(closeValue.GetString(), out double close))
                        {
                            closes.Add(close);
                        }
                    }
                    if (closes.Count == maxCount)
                        break;
                }
            }
            return closes;
        }

        // Simple momentum calculation: (latest close - oldest close) / oldest close
        private double CalculateMomentum(List<double> closes)
        {
            if (closes.Count < 2) return 0;
            var latest = closes[0];
            var oldest = closes[closes.Count - 1];
            return (latest - oldest) / oldest;
        }

        // Simple volatility calculation: standard deviation of closes
        private double CalculateVolatility(List<double> closes)
        {
            if (closes.Count == 0) return 0;
            var avg = closes.Average();
            var variance = closes.Sum(c => System.Math.Pow(c - avg, 2)) / closes.Count;
            return System.Math.Sqrt(variance);
        }
        
        private double RoundDouble(double value)
        {
            return Math.Round(value, 2);
        }

        private List<double> RoundDoubleList(List<double> values)
        {
            return values.Select(v => Math.Round(v, 2)).ToList();
        }
    }
}
