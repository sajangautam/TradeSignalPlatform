import { useState, useEffect } from 'react';
import ErrorAlert from './components/ErrorAlert';
import SignalForm from './components/SignalForm';
import SignalList from './components/SignalList';
import { TradeSignal } from './types';
import { getSignals, addSignal } from './api';

export default function App() {
  const [signals, setSignals] = useState<TradeSignal[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchSignals = async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await getSignals();
      setSignals(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Fetch failed');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchSignals();
  }, []);

  const handleAddSignal = async (ticker: string, action: string, price: number) => {
    try {
      setIsLoading(true);
      const newSignal = await addSignal({ ticker, action, price });
      setSignals([...signals, newSignal]);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Add failed');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="app">
      <h1>Trade Signal Manager</h1>
      <SignalForm onSubmit={handleAddSignal} disabled={isLoading} />
      {error ? (
        <ErrorAlert message={error} onRetry={fetchSignals} />
      ) : (
        <SignalList signals={signals} isLoading={isLoading} />
      )}
    </div>
  );
}