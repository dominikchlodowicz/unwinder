export interface HotelResponseOfferData {
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
  convertedCurrencyPrice: ConvertedCurrencyPrice;
}

interface Price {
  currency: string;
  total: string;
}

interface Room {
  typeEstimated: TypeEstimated;
}

interface TypeEstimated {
  category: string;
  beds: number;
}

interface ConvertedCurrencyPrice {
  currencyCode: string;
  value: number;
}
