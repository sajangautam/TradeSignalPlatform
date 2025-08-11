using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeSignalManager.Infrastructure.Services
{
    public static class TechnicalIndicators
    {
        // RSI: returns single double value representing the latest RSI
        public static double CalculateRSI(List<double> closes, int period = 14)
        {
            if (closes.Count < period + 1)
                return 0;

            double gain = 0;
            double loss = 0;

            for (int i = closes.Count - period; i < closes.Count; i++)
            {
                var change = closes[i] - closes[i - 1];
                if (change > 0)
                    gain += change;
                else
                    loss -= change;
            }

            if (loss == 0) return 100;

            double rs = gain / loss;
            return 100 - (100 / (1 + rs));
        }

        // SMA: simple moving average (single value)
        public static double CalculateSMA(List<double> closes, int period)
        {
            if (closes.Count < period)
                return 0;
            return closes.TakeLast(period).Average();
        }

        // EMA: exponential moving average (list of doubles)
        public static List<double> CalculateEMA(List<double> prices, int period)
        {
            List<double> emaValues = new List<double>();

            if (prices.Count < period)
                return emaValues;

            // Start with SMA for first EMA value
            double sma = prices.Take(period).Average();
            emaValues.Add(sma);

            double multiplier = 2.0 / (period + 1);

            for (int i = period; i < prices.Count; i++)
            {
                double ema = ((prices[i] - emaValues.Last()) * multiplier) + emaValues.Last();
                emaValues.Add(ema);
            }

            return emaValues;
        }

        // MACD: returns latest MACD line and Signal line (two lists)
        public static (List<double> macdLine, List<double> signalLine) CalculateMACD(List<double> prices, int shortPeriod = 12, int longPeriod = 26, int signalPeriod = 9)
        {
            var emaShort = CalculateEMA(prices, shortPeriod);
            var emaLong = CalculateEMA(prices, longPeriod);

            // MACD line = EMA(shortPeriod) - EMA(longPeriod)
            var macdLine = new List<double>();

            int count = Math.Min(emaShort.Count, emaLong.Count);
            for (int i = 0; i < count; i++)
            {
                macdLine.Add(emaShort[i] - emaLong[i]);
            }

            // Signal line = EMA of MACD line (signalPeriod)
            var signalLine = CalculateEMA(macdLine, signalPeriod);

            return (macdLine, signalLine);
        }

        // Bollinger Bands: returns middle, upper, lower bands (lists)
        public static (List<double> middleBand, List<double> upperBand, List<double> lowerBand) CalculateBollingerBands(List<double> closes, int period = 20, double stdDevMultiplier = 2)
        {
            if (closes.Count < period)
                return (new List<double>(), new List<double>(), new List<double>());

            List<double> middleBand = new List<double>();
            List<double> upperBand = new List<double>();
            List<double> lowerBand = new List<double>();

            for (int i = 0; i <= closes.Count - period; i++)
            {
                var window = closes.Skip(i).Take(period).ToList();
                double avg = window.Average();
                double stdDev = Math.Sqrt(window.Sum(x => Math.Pow(x - avg, 2)) / period);

                middleBand.Add(avg);
                upperBand.Add(avg + stdDevMultiplier * stdDev);
                lowerBand.Add(avg - stdDevMultiplier * stdDev);
            }
            return (middleBand, upperBand, lowerBand);
        }
    }
}
