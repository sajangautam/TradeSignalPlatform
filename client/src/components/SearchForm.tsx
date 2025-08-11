import React, { useState } from "react";
import "./SearchForm.css";

interface Props {
  onSearch: (symbol: string) => void;
}

const SearchForm: React.FC<Props> = ({ onSearch }) => {
  const [symbol, setSymbol] = useState("");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!symbol.trim()) return;
    onSearch(symbol.trim().toUpperCase());
  };

  return (
    <form className="search-form" onSubmit={handleSubmit}>
      <input
        type="text"
        className="search-input"
        value={symbol}
        onChange={(e) => setSymbol(e.target.value)}
        placeholder="Enter ticker symbol (e.g. AAPL)"
      />
      <button type="submit" className="search-button">
        Search
      </button>
    </form>
  );
};

export default SearchForm;
