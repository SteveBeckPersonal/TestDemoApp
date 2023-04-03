interface IForecast {
  date: Date;
  tempC: number;
  summary: string;
}

export class Forecast implements IForecast {
  date: Date;
  tempC: number;
  summary: string;

  constructor(date: Date, tempC: number, summary: string) {
    this.date = date;
    this.tempC = tempC;
    this.summary = summary;
  }
}
