import { TradeSignal } from '../types';

interface SignalListProps {
  signals: TradeSignal[];
  isLoading?: boolean;
}

export default function SignalList({ signals, isLoading }: SignalListProps) {
    if (isLoading) return <div className="loading-spinner">🌀 Loading...</div>;
  return (
    <div className="signal-list">
      <h2>Current Signals</h2>
      {signals.length === 0 ? (
        <p>No signals yet. Add one above!</p>
      ) : (
        <ul>
          {signals.map((signal) => (
            <li key={signal.id}>
              {signal.ticker} - {signal.action} @ ${signal.price.toFixed(2)}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}