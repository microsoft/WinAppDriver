/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import { PageObject, By2 } from "selenium-appium";

class CheckBoxPage extends PageObject {
  isPageLoaded() {
    return this.checkbox1Button.isDisplayed();
  }

  private get checkbox1Button() { return By2.nativeName('Two-state CheckBox'); }
  private get control1Ooutput() { return By2.nativeAccessibilityId('Control1Output');}

  clickCheckbox1() {
      return this.checkbox1Button.click();      
  }

  getControl1Output() {
      return this.control1Ooutput.getText();
  }
}

export default new CheckBoxPage();