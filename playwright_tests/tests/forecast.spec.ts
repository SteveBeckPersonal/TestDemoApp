import { test, expect } from "@playwright/test";
import { DemoSolution } from "../src/DemoSolution.app";
import { Forecast } from "../src/Models/Forecast";
import { v4 as uuidv4 } from "uuid";

test("Add forecast", async ({ page }) => {
  const forecast = new Forecast(new Date(), 5, `Playwright_${uuidv4()}`);

  const demoSolution = new DemoSolution(page);
  await demoSolution.forecasts.goto();
  await demoSolution.forecasts.addForecast(forecast);
  await demoSolution.forecasts.validateForecast(forecast);
  await demoSolution.forecasts.removeForecast(forecast);
});
