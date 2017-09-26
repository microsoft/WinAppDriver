@standard
Feature: Standard calculator
  As a mathematician, I might need help calculating and recalling numbers

  Scenario: Start and quit the application
    Given I start the application
     Then I see the result is "0"

  # Find the buttons by their names and click them in sequence to peform 1 + 7 = 8
  Scenario: Addition
    Given I start the application
     When I press "One"
      And I press "Plus"
      And I press "Seven"
      And I press "Equals"
     Then I see the result is "8"

  @accessibility
  # Find the buttons by their accessibility ids and click them in sequence to perform 88 / 11 = 8
  Scenario: Division (by accessibility ID)
    Given I start the application
     When I press "num8Button" by id
      And I press "num8Button" by id
      And I press "divideButton" by id
      And I press "num1Button" by id
      And I press "num1Button" by id
      And I press "equalButton" by id
     Then I see the result is "8"

  # Find the buttons by their names and click them in sequence to perform 9 x 9 = 81
  Scenario: Multiplication
    Given I start the application
     When I press "Nine"
      And I press "Multiply by"
      And I press "Nine"
      And I press "Equals"
     Then I see the result is "81"

  @accessibility
  # Find the buttons by their accessibility ids and click them in sequence to perform 9 - 1 = 8
  Scenario: Subtraction (by accessibility id)
    Given I start the application
     When I press "num9Button" by id
      And I press "minusButton" by id
      And I press "num1Button" by id
      And I press "equalButton" by id
     Then I see the result is "8"

  @template
  # Run sequence of button presses specified above and validate the results
  Scenario Outline: Subsequent calculations
    Given I start the application
     When I press "<left_operand>"
      And I press "<operator>"
      And I press "<right_operand>"
      And I press "Equals"
     Then I see the result is "<expected>"

Examples:
| left_operand | operator  | right_operand | expected |
| One          | Plus      | Seven         | 8        |
| Nine         | Minus     | One           | 8        |
| Eight        | Divide by | Eight         | 1        |
