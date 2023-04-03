import { expect, Locator, Page } from "@playwright/test";
export class Navbar {
  readonly page: Page;
  constructor(page: Page) {
    this.page = page;
  }
}
