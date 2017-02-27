
function Confirm-Host {
    param(
        [string]   $Title = $null,
        [string]   $Message = "Are you sure you want to continue?",
        [string[]] $Options = @("&Yes", "&No"),
        [int]      $DefaultOptionIndex = 0
    )

    $choices = new-object Collections.ObjectModel.Collection[Management.Automation.Host.ChoiceDescription]
    foreach ($opt in $Options) {
        $choices.Add((new-object Management.Automation.Host.ChoiceDescription -ArgumentList $opt))
    }

    $Host.UI.PromptForChoice($Title, $Message, $choices, $DefaultOptionIndex)
}

