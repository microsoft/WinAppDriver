/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import { BasePage, By2 } from './BasePage';

class HomePage extends BasePage {
	isPageLoaded() {
		return this.basicInputButton.isDisplayed();
	}

	private get basicInputButton() {
		return By2.nativeName('Basic Input');
	}

	gotoBasicInputPage() {
		return this.basicInputButton.click();
	}
}
export default new HomePage();
