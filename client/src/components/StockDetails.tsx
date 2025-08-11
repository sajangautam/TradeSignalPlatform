import React from 'react';
import './StockDetails.css';

interface StockDetailsProps {
  data: any;
}

const formatNumber = (num: number | null | undefined, decimals = 2) => {
  if (num === null || num === undefined) return 'N/A';
  return num.toFixed(decimals);
};

export const StockDetails: React.FC<StockDetailsProps> = ({ data }) => {
  return (
    <div className="stock-details-container">
      <h2 className="stock-details-title">
        {data.name} <span className="stock-details-symbol">({data.symbol})</span>
      </h2>

      <table className="stock-details-table">
        <tbody>
          <tr>
            <th>Exchange</th>
            <td>{data.exchange || 'N/A'}</td>
            <th>Sector</th>
            <td>{data.sector || 'N/A'}</td>
          </tr>
          <tr>
            <th>Current Price</th>
            <td>
              {data.currentPrice !== null
                ? <span className="stock-details-price">${formatNumber(data.currentPrice)}</span>
                : 'N/A'}
            </td>
            <th>Momentum</th>
            <td>
              {data.momentum !== undefined
                ? `${(data.momentum * 100).toFixed(2)}%`
                : 'N/A'}
            </td>
          </tr>
          <tr>
            <th>Volatility</th>
            <td>{data.volatility?.toFixed(4) ?? 'N/A'}</td>
            <th>RSI</th>
            <td>{data.rsi?.toFixed(2) ?? 'N/A'}</td>
          </tr>
          <tr>
            <th>SMA 20</th>
            <td>
              {data.sma20 && data.sma20 !== 0
                ? `$${formatNumber(data.sma20)}`
                : 'N/A'}
            </td>
            <th>EMA 12 (last)</th>
            <td>
              {data.emA12?.length
                ? `$${formatNumber(data.emA12.at(-1))}`
                : 'N/A'}
            </td>
          </tr>
          <tr>
            <th>MACD (last)</th>
            <td>{data.macd?.length ? data.macd.at(-1).toFixed(4) : 'N/A'}</td>
            <th>MACD Signal (last)</th>
            <td>{data.macdSignal?.length ? data.macdSignal.at(-1).toFixed(4) : 'N/A'}</td>
          </tr>
          <tr>
            <th>Bollinger Bands (last)</th>
            <td colSpan={3}>
              {data.bollingerBandsMiddle?.length ? (
                <div className="stock-details-bbands">
                  <div><strong>Middle:</strong> ${formatNumber(data.bollingerBandsMiddle.at(-1))}</div>
                  <div><strong>Upper:</strong> ${formatNumber(data.bollingerBandsUpper.at(-1))}</div>
                  <div><strong>Lower:</strong> ${formatNumber(data.bollingerBandsLower.at(-1))}</div>
                </div>
              ) : 'N/A'}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};
