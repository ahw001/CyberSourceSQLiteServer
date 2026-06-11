param(
  [string]$InputDir = ".",
  [int]$Size = 360
)

# Resolve inkscape from PATH or typical install
$inkscape = (Get-Command inkscape -ErrorAction SilentlyContinue).Source
if (-not $inkscape) {
  $candidates = @(
    "C:\Program Files\Inkscape\bin\inkscape.exe",
    "C:\Program Files\Inkscape\bin\inkscape.com",
    "$env:LOCALAPPDATA\Programs\Inkscape\bin\inkscape.exe"
  )
  $inkscape = $candidates | Where-Object { Test-Path $_ } | Select-Object -First 1
}
if (-not $inkscape) { throw "Inkscape CLI not found." }

# Sanity check version
$ver = (& $inkscape --version 2>&1)
Write-Host "Using: $ver" -ForegroundColor DarkGray

if (-not (Test-Path $InputDir)) { throw "Input directory not found: $InputDir" }

$svgs = Get-ChildItem -Path $InputDir -Filter *.svg -File
if (-not $svgs) { Write-Host "No SVG files found in $InputDir"; return }

foreach ($svg in $svgs) {
  $in  = $svg.FullName
  $out = Join-Path $svg.DirectoryName "$($svg.BaseName)_${Size}.png"
  Write-Host "Rendering: $($svg.Name) → $(Split-Path $out -Leaf)" -ForegroundColor Cyan

  # Minimal, known-good set for Inkscape ≥ 1.x
  $args = @(
    "--batch-process",
    "--export-type=png",
    "--export-filename=$out",
    "--export-width=$Size",
    "--export-height=$Size",
    "--export-background-opacity=0",
    "--export-overwrite",
    "--export-area-page",
    # Comment the next line if you suspect it causes issues on some SVGs:
    "--actions=select-all;object-to-path;",
    $in
  )

  # Capture stdout/stderr to help diagnose
  $output = & $inkscape @args 2>&1

  # Verify the file really exists (don’t trust exit code alone)
  if (Test-Path $out) {
    Write-Host "✅ Done: $out" -ForegroundColor Green
  } else {
    Write-Host "❌ Failed: $($svg.Name)" -ForegroundColor Red
    Write-Host "Inkscape output:" -ForegroundColor Yellow
    $output | ForEach-Object { Write-Host "  $_" }
    Write-Host "Hints: try removing --actions, or run once with only --export-width OR only --export-height." -ForegroundColor DarkYellow
  }
}

Write-Host "Finished."
