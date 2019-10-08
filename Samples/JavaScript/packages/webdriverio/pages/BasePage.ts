/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

export class BasePage {
	isPageLoaded(): boolean {
		return false;
	}

	waitForPageLoaded(timeout?: number) {
		browser.waitUntil(
			() => {
				return this.isPageLoaded();
			},
			this.timeoutForPageLoaded(timeout),
			'Wait for page ' + this.constructor.name + ' timeout'
		);
	}

	protected timeoutForPageLoaded(currentTimeout?: number) {
		if (currentTimeout) return currentTimeout;
		return this.waitforPageTimeout;
	}

	// Default timeout for waitForPageLoaded command in PageObject
	private waitforPageTimeout: number = 10000;
}

export class By2 {
	static nativeAccessibilityId(testId: string): WebdriverIO.Element {
		return $('~' + testId);
	}

	static nativeName(name: string): WebdriverIO.Element {
		return $("[name='" + name + "']");
	}
}
