import { expect, Locator, Page } from "@playwright/test";
export class CounterPage {
  readonly page: Page;
  constructor(page: Page) {
    this.page = page;
  }

  goto() {
    this.page.goto("/counter");
  }
}
