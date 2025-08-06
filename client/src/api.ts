import { TradeSignal } from './types';

const API_URL = 'http://localhost:5000/api/TradeSignal';

export async function getSignals(): Promise<TradeSignal[]> {
  const response = await fetch(API_URL);
  if (!response.ok) throw new Error('Failed to fetch signals');
  return response.json();
}

export async function addSignal(signal: Omit<TradeSignal, 'id' | 'timestamp'>): Promise<TradeSignal> {
  const response = await fetch(API_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(signal)
  });
  if (!response.ok) throw new Error('Failed to add signal');
  return response.json();
}