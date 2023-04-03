import { expect, Locator, Page } from "@playwright/test";
import { CounterPage } from "./Pages/counter.page";
import { ForecastPage } from "./Pages/weatherforecasts.page";
export class DemoSolution {
  readonly page: Page;
  readonly counter: CounterPage;
  readonly forecasts: ForecastPage;
  constructor(page: Page) {
    this.page = page;
    this.counter = new CounterPage(page);
    this.forecasts = new ForecastPage(page);
  }

  goto() {
    this.page.goto("/");
  }
}
