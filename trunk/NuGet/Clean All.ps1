##
##	Clean out all output folders
##  ============================
##

function Pause ($Message="Press any key to continue...")
{
    Write-Host -NoNewLine $Message
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    Write-Host ""
}

try
{
    ## Initialise
    ## ----------
    $originalBackground = $host.UI.RawUI.BackgroundColor
    $originalForeground = $host.UI.RawUI.ForegroundColor
    $originalLocation = Get-Location
    $packages = @("Caliburn.Micro WPF", "Caliburn.Micro WPF.V2", "CslaContrib", "CustomFieldData", "MEF", "ObjectCaching AppFabric", "Wisej", "Windows") ## Leave out "WPF" until it has some content.
    ## $packages = @("Caliburn.Micro WPF", "Caliburn.Micro WPF.V2", "CslaContrib", "CustomFieldData", "MEF", "ObjectCaching AppFabric", "Wisej", "Windows", "WPF")

    $host.UI.RawUI.BackgroundColor = [System.ConsoleColor]::Black
    $host.UI.RawUI.ForegroundColor = [System.ConsoleColor]::White

    Write-Host "Clean all CslaContrib NuGet build outputs" -ForegroundColor White
    Write-Host "=========================================" -ForegroundColor White

    ## NB - Cleanup destination package folder
    ## ---------------------------------------
    Write-Host "Clean destination folders..." -ForegroundColor Yellow
    Remove-Item ".\Packages\*.nupkg" -Recurse -Force -ErrorAction SilentlyContinue

    ## Spawn off individual create processes...
    ## ---------------------------------------
    Set-Location "$originalLocation\Definition" ## Adjust current working directory since scripts are using relative paths
    $packages | ForEach { & ".\Clean.ps1" $_ }
    Write-Host "Clean all done." -ForegroundColor Green
}
catch
{
    $baseException = $_.Exception.GetBaseException()
    if ($_.Exception -ne $baseException)
    {
      Write-Host $baseException.Message -ForegroundColor Magenta
    }
    Write-Host $_.Exception.Message -ForegroundColor Magenta
    Pause
}
finally
{
    ## Restore original values
    $host.UI.RawUI.BackgroundColor = $originalBackground
    $host.UI.RawUI.ForegroundColor = $originalForeground
    Set-Location $originalLocation
}
Pause # For debugging purposes