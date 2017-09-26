Before do |scenario|
    # The +scenario+ argument is optional, but if you use it, you can get the title,
    # description, or name (title + description) of the scenario that is about to be
    # executed.
    name = "#{scenario.name}"
end

After do |scenario|
    # Do something after each scenario.
    # The +scenario+ argument is optional, but
    # if you use it, you can inspect status with
    # the #failed?, #passed? and #exception methods.
    $CalculatorSession.quit

    if scenario.failed?
        reason = "#{scenario.exception.message}"
        print reason
    end
end
