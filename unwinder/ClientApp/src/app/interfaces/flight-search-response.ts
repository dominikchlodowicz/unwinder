interface MetaObject {
  count: number;
}

interface DepartureOutput {
  iataCode: string;
  at: string;
}

interface ArrivalOutput {
  iataCode: string;
  at: string;
}

interface SegmentObject {
  departure: DepartureOutput;
  arrival: ArrivalOutput;
  duration: string;
  carrierCode: string;
  number: string;
}

interface Itinerary {
  duration: string;
  segments: SegmentObject[];
}

interface PriceObject {
  total: string;
  currency: string;
}

interface FlightOfferData {
  itineraries: Itinerary[];
  price: PriceObject;
}

export interface FlightSearchResponse {
  meta: MetaObject;
  data: FlightOfferData[];
}
