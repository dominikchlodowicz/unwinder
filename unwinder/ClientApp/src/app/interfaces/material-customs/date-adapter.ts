export interface IMondayCustomDateAdapterService {
  getFirstDayOfWeek(): number;
}

export interface ICustomEuropeDateFormatService {
  parse(value: any): Date | null;
}
