import assert from 'assert';

function getCalculatorResults() {
	return $('~CalculatorResults')
		.getText()
		.replace('Display is', '')
		.trim();
}

describe('CalculatorTest', () => {
	beforeEach(() => {
		$('~clearButton').click();
	});
	// Find the buttons by their accessibility ids and click them in sequence to perform 88 / 11 = 8
	it('Division', () => {
		$('~num8Button').click();
		$('~num8Button').click();
		$('~divideButton').click();
		$('~num1Button').click();
		$('~num1Button').click();
		$('~equalButton').click();

		assert.equal(getCalculatorResults(), '8');
	});

	// Find the buttons by their accessibility ids and click them in sequence to perform 8*8 = 64
	it('Multiplication', () => {
		$('~num8Button').click();
		$('~multiplyButton').click();
		$('~num8Button').click();
		$('~equalButton').click();

		assert.equal(getCalculatorResults(), '64');
	});
});
