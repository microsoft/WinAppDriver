/**
 * Copyright (c) Microsoft Corporation. All rights reserved.
 * Licensed under the MIT License.
 */

import assert from 'assert';
import HomePage from '../pages/HomePage';
import BasicInputPage from '../pages/BasicInputPage';
import ButtonPage from '../pages/ButtonPage';
import CheckBoxPage from '../pages/CheckBoxPage';

describe('Samples', () => {
	beforeEach(() => {
		HomePage.gotoBasicInputPage();
		BasicInputPage.waitForPageLoaded();
	});

	it('ButtonPage', () => {
		BasicInputPage.gotoButtonPage();
		ButtonPage.waitForPageLoaded();

		ButtonPage.clickButton1();
		assert.equal(ButtonPage.getControl1Output(), 'You clicked: Button1');
	});

	it('checkboxPage', () => {
		BasicInputPage.gotoCheckboxPage();
		CheckBoxPage.waitForPageLoaded();

		CheckBoxPage.clickCheckbox1();
		assert.equal(CheckBoxPage.getControl1Output(), 'You checked the box.');
	});
});
