import React, { useState } from 'react';
import SearchForm from './components/SearchForm';
import { StockDetails } from './components/StockDetails';
import './App.css';

const App: React.FC = () => {
  const [stockData, setStockData] = useState<any | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSearch = async (symbol: string) => {
    setLoading(true);
    setError(null);
    setStockData(null);

    try {
      const response = await fetch(`http://localhost:5000/api/marketdata/search?symbol=${symbol}`);
      if (response.status === 404) {
      setError("Not an S&P 500 stock");
      return;
    }
      const data = await response.json();
      setStockData(data);
    } catch (err: any) {
      setError(err.message || 'Failed to fetch data');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="app-container">
      <h1 className="app-title">📈 S&P 500 Stock Analysis</h1>
      <SearchForm onSearch={handleSearch} />
      {loading && (
        <div className="loading-container">
          <div className="spinner"></div>
          <p>Loading...</p>
        </div>
      )} 
     {error && (
        <div className="error-wrapper">
          <div className="error-card">
            <span className="error-icon">⚠️</span>
            <p>{error}</p>
          </div>
        </div>
      )}
      {stockData && <StockDetails data={stockData} />}
    </div>
  );
};

export default App;
