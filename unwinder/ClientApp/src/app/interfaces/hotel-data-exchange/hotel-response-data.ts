interface Average {
  base: string;
}

interface Change {
  startDate: string;
  endDate: string;
  base: string;
}

interface Datum {
  type: string;
  hotel: Hotel;
  available: boolean;
  offers: Offer[];
  self: string;
}

interface Hotel {
  hotelId: string;
  name: string;
  latitude: number;
  longitude: number;
}

interface Offer {
  id: string;
  checkInDate: string;
  checkOutDate: string;
  room: Room;
  price: Price;
}

interface Price {
  currency: string;
  total: string;
}

interface Room {
  typeEstimated: TypeEstimated;
}

export interface HotelResponseData {
  data: Datum[];
  dictionaries: Dictionaries;
  convertedCurrencyPrice: ConvertedCurrencyPrice;
}

interface Dictionaries {
  currencyConversionLookupRates: { [key: string]: CurrencyConversion };
}

interface CurrencyConversion {
  rate: string;
  target: string;
  targetDecimalPlaces: number;
}

interface TypeEstimated {
  category: string;
}

interface ConvertedCurrencyPrice {
  currencyCode: string;
  value: number;
}
