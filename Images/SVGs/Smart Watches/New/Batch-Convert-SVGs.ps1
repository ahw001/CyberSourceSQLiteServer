<#
.SYNOPSIS
  Convert all SVGs in a directory to PNG using rsvg-convert (librsvg).

.PARAMETER InputDir
  Folder that contains .svg files. Defaults to current directory.

.PARAMETER OutputDir
  Folder to write .png files. Defaults to "<InputDir>\_png".

.PARAMETER Width
  Target width in pixels. If only Width is given, height scales proportionally.

.PARAMETER Height
  Target height in pixels. If both Width and Height are given, image is resized to both.

.PARAMETER Overwrite
  Overwrite existing .png files if they exist.

.PARAMETER Dpi
  Optional DPI hint for scaling (rarely needed). Example: 96, 192, etc.

.NOTES
  Requires rsvg-convert.exe (librsvg). Common path on Windows:
  C:\msys64\mingw64\bin\rsvg-convert.exe
#>

param(
  [string]$InputDir = ".",
  [string]$OutputDir,
  [int]$Width = 260,
  [int]$Height = 475,
  [switch]$Overwrite,
  [int]$Dpi
)

# --- Resolve directories
$InputDir  = (Resolve-Path $InputDir).Path
if (-not $OutputDir) { $OutputDir = Join-Path $InputDir "_png" }
if (-not (Test-Path $OutputDir)) { New-Item -ItemType Directory -Path $OutputDir | Out-Null }

# --- Find rsvg-convert
function Get-RsvgConvertPath {
  # 1) PATH
  $fromPath = (Get-Command rsvg-convert -ErrorAction SilentlyContinue)?.Source
  if ($fromPath) { return $fromPath }

  # 2) Common MSYS2 install
  $common = "C:\msys64\mingw64\bin\rsvg-convert.exe"
  if (Test-Path $common) { return $common }

  # 3) Ask user to install MSYS2 + librsvg
  throw "rsvg-convert not found. Install MSYS2 and run: pacman -S mingw-w64-x86_64-librsvg
  Then ensure C:\msys64\mingw64\bin is in your PATH."
}

$rsvg = Get-RsvgConvertPath
Write-Host "Using rsvg-convert at: $rsvg"

# --- Build base args for rsvg-convert
function Build-RsvgArgs {
  param(
    [string]$InSvg,
    [string]$OutPng
  )

  $args = @()
  $args += @("$InSvg")       # input SVG
  $args += @("-f","png")     # output format
  $args += @("-o", "$OutPng")# output file

  if ($Width -gt 0)  { $args += @("-w", "$Width") }
  if ($PSBoundParameters.ContainsKey('Height') -and $Height -gt 0) { $args += @("-h", "$Height") }
  if ($PSBoundParameters.ContainsKey('Dpi') -and $Dpi -gt 0) {
    $args += @("--dpi-x", "$Dpi", "--dpi-y", "$Dpi")
  }

  # Transparent background by default (don’t set -b)
  return ,$args
}

# --- Process files
$svgs = Get-ChildItem -Path $InputDir -Filter *.svg -File -Recurse:$false
if (-not $svgs) {
  Write-Warning "No .svg files found in $InputDir"
  return
}

$ok = 0; $fail = 0
foreach ($svg in $svgs) {
  $out = Join-Path $OutputDir ([IO.Path]::GetFileNameWithoutExtension($svg.Name) + ".png")

  if ((-not $Overwrite) -and (Test-Path $out)) {
    Write-Host "⏭️  Skipping (exists): $out"
    continue
  }

  $args = Build-RsvgArgs -InSvg $svg.FullName -OutPng $out

  try {
    & "$rsvg" @args 2>$null
    if ($LASTEXITCODE -eq 0 -and (Test-Path $out)) {
      Write-Host "✅ Converted: $($svg.Name) -> $(Split-Path $out -Leaf)"
      $ok++
    } else {
      Write-Warning "⚠️  Failed: $($svg.Name)"
      $fail++
    }
  } catch {
    Write-Warning "❌ Error converting $($svg.Name): $($_.Exception.Message)"
    $fail++
  }
}

Write-Host "Done. Success: $ok, Failed: $fail. Output: $OutputDir"
