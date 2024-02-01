export interface ConcreteFlight {
  itineraries: Itinerary[];
  price: Price;
}

export interface Itinerary {
  duration: string;
  segments: Segment[];
}

export interface Segment {
  departure: Departure;
  arrival: Arrival;
  duration: string;
  carrierCode: string;
  number: string;
}

export interface Departure {
  iataCode: string;
  at: string;
}

export interface Arrival {
  iataCode: string;
  at: string;
}

export interface Price {
  total: string;
  currency: string;
}
