import { useState } from 'react';

interface SignalFormProps {
  onSubmit: (ticker: string, action: string, price: number) => void;
  disabled?: boolean;
}

export default function SignalForm({ onSubmit, disabled }: SignalFormProps) {
  const [ticker, setTicker] = useState('');
  const [action, setAction] = useState('BUY');
  const [price, setPrice] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(ticker, action, parseFloat(price));
  };

  return (
    <form onSubmit={handleSubmit} className="signal-form">
      <input
        type="text"
        value={ticker}
        onChange={(e) => setTicker(e.target.value.toUpperCase())}
        placeholder="Ticker (e.g., AAPL)"
        required
        disabled={disabled} // Add this
      />
      <select 
        value={action} 
        onChange={(e) => setAction(e.target.value)}
        disabled={disabled} // Add this
      >
        <option value="BUY">Buy</option>
        <option value="SELL">Sell</option>
      </select>
      <input
        type="number"
        step="0.01"
        value={price}
        onChange={(e) => setPrice(e.target.value)}
        placeholder="Price"
        required
        disabled={disabled} // Add this
      />
      <button 
        type="submit" 
        disabled={disabled} // Add this
      >
        {disabled ? 'Processing...' : 'Add Signal'}
      </button>
    </form>
  );
}