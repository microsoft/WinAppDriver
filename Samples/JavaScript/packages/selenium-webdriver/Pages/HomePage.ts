/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import { PageObject, By2 } from "selenium-appium";

class HomePage extends PageObject {
  isPageLoaded() {
    return this.basicInputButton.isDisplayed();
  }
  private get basicInputButton() { return By2.nativeName('Basic Input'); }

  gotoBasicInputPage() {
    return this.basicInputButton.click();
  }
}

export default new HomePage();