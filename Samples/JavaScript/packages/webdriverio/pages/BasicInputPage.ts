/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import { BasePage, By2 } from './BasePage';

class BasicInputPage extends BasePage {
	isPageLoaded() {
		return this.checkBoxButton.isDisplayed();
	}

	private get checkBoxButton() {
		return By2.nativeName('CheckBox');
	}
	private get buttonButton() {
		return By2.nativeName('Button');
	}

	gotoButtonPage() {
		return this.buttonButton.click();
	}

	gotoCheckboxPage() {
		return this.checkBoxButton.click();
	}
}

export default new BasicInputPage();
