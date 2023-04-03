import { expect, Locator, Page } from "@playwright/test";
import { Forecast } from "../Models/Forecast";
import exp from "constants";
export class ForecastPage {
  readonly page: Page;
  readonly date: Locator;
  readonly temp: Locator;
  readonly summary: Locator;
  readonly saveBtn: Locator;

  constructor(page: Page) {
    this.page = page;
    this.date = page.getByLabel("Date");
    this.temp = page.locator("#Temp");
    this.summary = page.getByLabel("Summary");
    this.saveBtn = page.getByRole("button", { name: "Save" });
  }

  async goto() {
    await this.page.goto("/");
  }

  async validateForecast(forecast: Forecast) {
    await expect
      .soft(this.page.getByRole("cell", { name: forecast.summary }))
      .toBeVisible();
    await expect
      .soft(
        this.page.getByRole("cell", {
          name: forecast.tempC.toString(),
          exact: true,
        })
      )
      .toBeVisible();
  }

  async addForecast(forecast: Forecast) {
    const formattedDate = forecast.date.toLocaleDateString("en-CA", {
      year: "numeric",
      month: "2-digit",
      day: "2-digit",
    });
    await this.date.fill(formattedDate);
    await this.temp.fill(forecast.tempC.toString());
    await this.summary.fill(forecast.summary);
    await this.saveBtn.click();
  }

  async removeForecast(forecast: Forecast) {
    const rowLocator = this.page
      .locator(`td:has-text("${forecast.summary}")`)
      .locator("..");
    const removeButtonLocator = rowLocator.locator("button#remove");
    await removeButtonLocator.click();
  }
}
