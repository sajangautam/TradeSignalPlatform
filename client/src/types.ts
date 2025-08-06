export interface TradeSignal {
  id: number;
  ticker: string;
  action: string;
  price: number;
  timestamp: string;
  notes?: string;
}