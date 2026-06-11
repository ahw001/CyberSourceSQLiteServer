<#
Batch-Export-Inkscape-MultiSize.ps1
Converts all SVGs in the current directory to PNGs at multiple resolutions
using Inkscape’s command-line export. Maintains transparency.
#>

# Define output sizes (width x height)
$Sizes = @(
    @{ Width = 1024; Height = 768;  Suffix = "small" },
    @{ Width = 2048; Height = 1536; Suffix = "medium" },
    @{ Width = 4096; Height = 3072; Suffix = "large" }
)

# Create output folder
$OutDir = "PNGs"
if (-not (Test-Path $OutDir)) { New-Item -ItemType Directory -Path $OutDir | Out-Null }

# Loop through SVGs
Get-ChildItem . -Filter *.svg | ForEach-Object {
    $svg = $_.FullName
    $base = [IO.Path]::GetFileNameWithoutExtension($_.Name)

    foreach ($sz in $Sizes) {
        $outFile = Join-Path $OutDir ("{0}{1}.png" -f $base, $sz.Suffix)
        Write-Host "Exporting $($svg) → $($outFile) [$($sz.Width)x$($sz.Height)]"

        & inkscape $svg `
            --export-type=png `
            --export-filename=$outFile `
            --export-width=$($sz.Width) `
            --export-height=$($sz.Height) `
            --export-background-opacity=0 | Out-Null
    }
}

Write-Host "`n✅ Done! PNGs created in: $OutDir"
