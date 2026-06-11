<#
Build-MobileFirst-Phone.ps1
Generates a fictional smartphone series (MobileFirst Pro Max) SVG pack and zips it:
- 16 front variants (4 colorways × 4 sensors) with UI
- 4 back variants
- 1 hero (angled)
- 1 exploded
- 2 clean (no-UI)
- README + brand-colors.json

Outputs:
- .\MobileFirst_Phone_SVG_Variants\
- .\MobileFirst_Phone_Variants.zip
#>

$OutDir = Join-Path (Get-Location) "MobileFirst_Phone_SVG_Variants"
if (-not (Test-Path $OutDir)) { New-Item -ItemType Directory -Path $OutDir | Out-Null }

# Global toggle: when $true, all SVGs export on a transparent canvas (no background rect)
$TransparentOutput = $true

# Brand config
$Brand = @{
  brand_name = "MobileFirst"
  series_name = "Pro Max"
  accent = "#4da3ff"
  font_stack = "system-ui, -apple-system, Segoe UI, Roboto, Ubuntu, Cantarell, 'Helvetica Neue', Arial, sans-serif"
  colorways = @{
    titanium = @{ frameTop="#c9ccd1"; frameSide="#9ea3aa"; frameEdge="#71757b"; bezel="#0a0a0c"; glassTop="#1f1f22"; glassBottom="#0b0b0d"; bg="#0b0c10"; text="#e9edf2" }
    black    = @{ frameTop="#3b3d41"; frameSide="#2a2c2f"; frameEdge="#1b1d20"; bezel="#050607"; glassTop="#0a0a0b"; glassBottom="#000000"; bg="#0b0c10"; text="#e9edf2" }
    blue     = @{ frameTop="#cfe2ff"; frameSide="#8fb3e8"; frameEdge="#4b6ea8"; bezel="#070a0e"; glassTop="#111726"; glassBottom="#0a0f1b"; bg="#0b0c10"; text="#e9edf2" }
    gold     = @{ frameTop="#f4e7cf"; frameSide="#e0c89a"; frameEdge="#a4844b"; bezel="#0a0907"; glassTop="#1b1710"; glassBottom="#0c0a06"; bg="#f6f7fb"; text="#14161a" }
  }
}

function Get-StyleBlock {
@'
    <style><![CDATA[
      /* Variables are applied on the root element via CSS custom properties. */
      .uiText { font-family: system-ui, -apple-system, Segoe UI, Roboto, Ubuntu, Cantarell, "Helvetica Neue", Arial, sans-serif; }
      .btnFill { fill:#6e737a; }
      .sensorFill { fill:#3b3f46; }
      /* higher contrast text — crisp on dark glass */
      .hiContrast {
        paint-order: stroke fill;
        stroke: rgba(0,0,0,.65);
        stroke-width: 3px;
      }
    ]]></style>
'@
}

$DefsBlock = @"
  <linearGradient id="gradFrame" x1="0" y1="0" x2="0" y2="1">
    <stop offset="0%" stop-color="var(--frameTop)"/>
    <stop offset="55%" stop-color="var(--frameSide)"/>
    <stop offset="100%" stop-color="var(--frameEdge)"/>
  </linearGradient>
  <linearGradient id="gradGlass" x1="0" y1="0" x2="0" y2="1">
    <stop offset="0%" stop-color="var(--glassTop)"/>
    <stop offset="100%" stop-color="var(--glassBottom)"/>
  </linearGradient>
  <filter id="innerShadow" x="-20%" y="-20%" width="140%" height="140%">
    <feOffset dy="8"/>
    <feGaussianBlur stdDeviation="12" result="b"/>
    <feComposite in="SourceAlpha" operator="arithmetic" k2="-1" k3="1"/>
    <feColorMatrix type="matrix" values="0 0 0 0 0  0 0 0 0 0  0 0 0 0 0  0 0 0 .55 0"/>
    <feBlend in2="SourceGraphic" mode="multiply"/>
  </filter>
  <filter id="drop" x="-20%" y="-20%" width="140%" height="140%">
    <feDropShadow dx="0" dy="24" stdDeviation="32" flood-color="#000" flood-opacity=".25"/>
  </filter>
  <linearGradient id="gloss" x1="0" y1="0" x2="1" y2="1">
    <stop offset="0%" stop-color="#fff" stop-opacity=".18"/>
    <stop offset="30%" stop-color="#fff" stop-opacity=".05"/>
    <stop offset="100%" stop-color="#fff" stop-opacity="0"/>
  </linearGradient>
  <linearGradient id="reflect" x1="0" y1="0" x2="0" y2="1">
    <stop offset="0%" stop-color="#fff" stop-opacity=".12"/>
    <stop offset="65%" stop-color="#fff" stop-opacity=".02"/>
    <stop offset="100%" stop-color="#fff" stop-opacity="0"/>
  </linearGradient>
  <radialGradient id="brandGlow" cx="50%" cy="38%" r="60%">
    <stop offset="0%" stop-color="var(--accent)" stop-opacity=".25"/>
    <stop offset="100%" stop-color="var(--accent)" stop-opacity="0"/>
  </radialGradient>
"@

# Phone device: 1000 x 2180 content area (scaled into 1400x2400 artboard)
function Get-PhoneFront([string]$Sensor, [bool]$IncludeUI=$true) {
  $sensorSvg = switch ($Sensor) {
    "pill"     { '<rect x="560" y="82" width="80" height="16" rx="8" class="sensorFill" opacity=".75"/>' }
    "hole"     { '<circle cx="605" cy="90" r="9" class="sensorFill" opacity=".85"/>' }
    "teardrop" { '<path d="M605 80 c10 10 10 26 0 36 a18 18 0 1 1 -0.1 -36z" class="sensorFill" opacity=".82"/>' }
    default    { '' }
  }

  $ui = if ($IncludeUI) {
@'
      <rect x="42" y="42" rx="84" ry="84" width="916" height="2096" fill="url(#brandGlow)"/>
      <g transform="translate(120, 880)" class="uiText hiContrast">
        <text x="0" y="0" font-size="120" font-weight="800" fill="var(--text)">MobileFirst</text>
        <text x="0" y="140" font-size="88" font-weight="700" fill="var(--text)">Pro Max</text>
        <text x="0" y="250" font-size="44" font-weight="500" fill="var(--text)" opacity=".85">6.9 in edge-to-edge • 120Hz • Titanium-tone frame</text>
      </g>
      <g transform="translate(260, 1420)" class="uiText hiContrast">
        <rect x="0" y="0" rx="28" ry="28" width="480" height="96" fill="rgba(0,0,0,0.28)" stroke="rgba(255,255,255,0.18)"/>
        <text x="240" y="64" text-anchor="middle" font-size="42" font-weight="700" fill="var(--text)">Preorder</text>
      </g>
'@
  } else { "" }

@"
    <g filter="url(#drop)">
      <rect x="0" y="0" rx="112" ry="112" width="1000" height="2180" fill="url(#gradFrame)"/>
      <rect x="30" y="30" rx="96" ry="96" width="940" height="2120" fill="var(--bezel)"/>
      <rect x="42" y="42" rx="84" ry="84" width="916" height="2096" fill="url(#gradGlass)" filter="url(#innerShadow)"/>
      <rect x="84" y="84" width="832" height="180" rx="48" fill="url(#reflect)"/>
      <path d="M120,280 C560,40 760,160 980,0 L980,0 L980,2120 L120,2120 Z" fill="url(#gloss)" opacity=".4"/>
      <rect x="-8" y="520" width="12" height="170" rx="6" class="btnFill"/>
      <rect x="-8" y="720" width="12" height="170" rx="6" class="btnFill"/>
      <rect x="996" y="760" width="12" height="220" rx="6" class="btnFill"/>
      $sensorSvg
    </g>
    $ui
"@
}

function Get-PhoneBack() {
@'
    <g filter="url(#drop)">
      <rect x="0" y="0" rx="112" ry="112" width="1000" height="2180" fill="url(#gradFrame)"/>
      <rect x="20" y="20" rx="100" ry="100" width="960" height="2140" fill="rgba(255,255,255,0.03)"/>
      <!-- Camera island (generic, distinct layout) -->
      <g transform="translate(140,160)">
        <rect x="0" y="0" rx="56" ry="56" width="320" height="280" fill="#14171b" opacity=".9"/>
        <circle cx="90" cy="90" r="54" fill="#0c0f12" stroke="#1f2329" stroke-width="6"/>
        <circle cx="230" cy="90" r="54" fill="#0c0f12" stroke="#1f2329" stroke-width="6"/>
        <circle cx="160" cy="200" r="54" fill="#0c0f12" stroke="#1f2329" stroke-width="6"/>
        <circle cx="300" cy="210" r="22" fill="#fbf7d6"/>
        <circle cx="36" cy="212" r="10" fill="#2a2f36"/>
      </g>
      <rect x="-8" y="520" width="12" height="170" rx="6" class="btnFill"/>
      <rect x="-8" y="720" width="12" height="170" rx="6" class="btnFill"/>
      <rect x="996" y="760" width="12" height="220" rx="6" class="btnFill"/>
    </g>
    <g class="uiText hiContrast" transform="translate(500, 1120)">
      <text text-anchor="middle" font-size="72" fill="var(--text)">MobileFirst</text>
      <text y="86" text-anchor="middle" font-size="44" fill="var(--text)" opacity=".9">Pro Max</text>
    </g>
'@
}

function Wrap-SVG([string]$Inner, [hashtable]$CW, [string]$Title, [Nullable[bool]]$IncludeBG=$null) {
  $style = Get-StyleBlock

  # Derive background from global flag if not provided
  if ($null -eq $IncludeBG) { $include = -not $TransparentOutput } else { $include = $IncludeBG }
  $bg = if ($include) { '<rect width="1400" height="2400" fill="var(--bg)"/>' } else { '' }

  # CSS variables directly on <svg> (single line)
  $svgVars = ("--frameTop:{0}; --frameSide:{1}; --frameEdge:{2}; --bezel:{3}; --glassTop:{4}; --glassBottom:{5}; --bg:{6}; --text:{7}; --accent:{8};" -f `
    $CW.frameTop, $CW.frameSide, $CW.frameEdge, $CW.bezel, $CW.glassTop, $CW.glassBottom, $CW.bg, $CW.text, $Brand.accent)

@"
<?xml version="1.0" encoding="UTF-8"?>
<!-- $Title | Generated $(Get-Date -Format yyyy-MM-dd) -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 2400" width="1400" height="2400" style="$svgVars">
  $style
  <defs>$DefsBlock</defs>
  $bg
  <g transform="translate(200,110) scale(1.0)">
    $Inner
  </g>
</svg>
"@
}

function Write-Text([string]$Path, [string]$Content) {
  $Content | Out-File -FilePath $Path -Encoding UTF8
}

# Build: 4 colorways × 4 sensors (front with UI)
$sensors = @('pill','hole','teardrop',$null)
$cws = 'titanium','black','blue','gold'
$created = @()

foreach ($cwName in $cws) {
  $cw = $Brand.colorways.$cwName
  foreach ($s in $sensors) {
    $sensorName = if ($null -eq $s) { 'none' } else { $s }
    $title = "Phone Front • $($cwName.ToUpperInvariant()) • Sensor: $sensorName"
    $inner = Get-PhoneFront -Sensor $s -IncludeUI $true
    $svg = Wrap-SVG -Inner $inner -CW $cw -Title $title  # IncludeBG derived from $TransparentOutput
    $fname = ('mobilefirst_phone_front_{0}_{1}.svg' -f $cwName, $sensorName)
    $path = Join-Path $OutDir $fname
    Write-Text $path $svg
    $created += $path
  }
}

# Back (4)
foreach ($cwName in $cws) {
  $cw = $Brand.colorways.$cwName
  $title = "Phone Back • $($cwName.ToUpperInvariant())"
  $inner = Get-PhoneBack
  $svg = Wrap-SVG -Inner $inner -CW $cw -Title $title
  $path = Join-Path $OutDir ("mobilefirst_phone_back_{0}.svg" -f $cwName)
  Write-Text $path $svg
  $created += $path
}

# Helper: inline CSS var block for manual SVGs
function Get-SvgVars([hashtable]$cw, [string]$accent) {
  ("--frameTop:{0}; --frameSide:{1}; --frameEdge:{2}; --bezel:{3}; --glassTop:{4}; --glassBottom:{5}; --bg:{6}; --text:{7}; --accent:{8};" -f `
    $cw.frameTop, $cw.frameSide, $cw.frameEdge, $cw.bezel, $cw.glassTop, $cw.glassBottom, $cw.bg, $cw.text, $accent)
}

# Hero (titanium, pill) — manual SVG with variables on <svg>
$cw = $Brand.colorways.titanium
$styleHero = Get-StyleBlock
$svgVarsHero = Get-SvgVars $cw $Brand.accent
$heroBG = if (-not $TransparentOutput) { '<rect width="1400" height="2400" fill="var(--bg)"/>' } else { '' }
$hero = @"
<?xml version="1.0" encoding="UTF-8"?>
<!-- Phone Hero • Titanium • Pill -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 2400" width="1400" height="2400" style="$svgVarsHero">
  $styleHero
  <defs>$DefsBlock</defs>
  $heroBG
  <g opacity=".45" transform="translate(420,240) rotate(-8) scale(0.9) translate(-300,-100)">$(Get-PhoneBack)</g>
  <g transform="translate(680,160) skewX(-8) rotate(12) scale(1.02) translate(-300,-100)">$(Get-PhoneFront -Sensor 'pill' -IncludeUI $true)</g>
</svg>
"@
$heroPath = Join-Path $OutDir "mobilefirst_phone_hero_titanium_pill.svg"
Write-Text $heroPath $hero
$created += $heroPath

# Exploded (black, hole) — manual SVG with variables on <svg>
$cw = $Brand.colorways.black
$styleExpl = Get-StyleBlock
$svgVarsExpl = Get-SvgVars $cw $Brand.accent
$explBG = if (-not $TransparentOutput) { '<rect width="1400" height="2400" fill="var(--bg)"/>' } else { '' }
$expl = @"
<?xml version="1.0" encoding="UTF-8"?>
<!-- Phone Exploded • Black • Hole -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 2400" width="1400" height="2400" style="$svgVarsExpl">
  $styleExpl
  <defs>$DefsBlock</defs>
  $explBG
  <g transform="translate(700,200) translate(-300,-100)">$(Get-PhoneFront -Sensor $null -IncludeUI $false)</g>
  <g transform="translate(700,60) translate(-300,-100)"><rect x="42" y="42" rx="84" ry="84" width="916" height="2096" fill="url(#gradGlass)" filter="url(#innerShadow)"/></g>
  <g transform="translate(700,10) translate(-300,-100)"><circle cx="605" cy="90" r="9" class="sensorFill" opacity=".85"/></g>
  <g transform="translate(700,-40) translate(-300,-100)">
    <rect x="42" y="42" rx="84" ry="84" width="916" height="2096" fill="url(#brandGlow)"/>
    <g transform="translate(120, 880)" class="uiText hiContrast">
      <text x="0" y="0" font-size="120" font-weight="800" fill="var(--text)">MobileFirst</text>
      <text x="0" y="140" font-size="88" font-weight="700" fill="var(--text)">Pro Max</text>
    </g>
  </g>
</svg>
"@
$explPath = Join-Path $OutDir "mobilefirst_phone_exploded_black_hole.svg"
Write-Text $explPath $expl
$created += $explPath

# Clean: front blue (hole), back gold
$cw = $Brand.colorways.blue
$inner = Get-PhoneFront -Sensor "hole" -IncludeUI $false
$svg = Wrap-SVG -Inner $inner -CW $cw -Title "Phone Clean • Blue • Front"
$cleanFront = Join-Path $OutDir "mobilefirst_phone_clean_front_blue_hole.svg"
Write-Text $cleanFront $svg
$created += $cleanFront

$cw = $Brand.colorways.gold
$inner = Get-PhoneBack
$svg = Wrap-SVG -Inner $inner -CW $cw -Title "Phone Clean • Gold • Back"
$cleanBack = Join-Path $OutDir "mobilefirst_phone_clean_back_gold.svg"
Write-Text $cleanBack $svg
$created += $cleanBack

# README + brand-colors.json
$readme = @"
MobileFirst Pro Max — Separate SVG Variant Pack
Generated on $(Get-Date -Format yyyy-MM-dd)

Contents:
- 16 front variants (4 colorways × 4 sensor styles) with UI
- 4 back variants
- 1 hero (angled)
- 1 exploded
- 2 clean variants (no UI)

Editing:
- Each SVG exposes CSS vars on the root element:
  --frameTop, --frameSide, --frameEdge, --bezel, --glassTop, --glassBottom, --bg, --text, --accent
- Typography via the .uiText font stack. High-contrast text via .hiContrast.

Notes:
- Fictional phone illustration; avoids replicating specific brand trade dress.
- Vector-based; scale to any resolution.
"@
$readme | Out-File (Join-Path $OutDir "README.txt") -Encoding UTF8

($Brand | ConvertTo-Json -Depth 6) | Out-File (Join-Path $OutDir "brand-colors.json") -Encoding UTF8

# ZIP everything
Add-Type -AssemblyName System.IO.Compression.FileSystem
$ZipPath = Join-Path (Get-Location) "MobileFirst_Phone_Variants.zip"
if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }
[System.IO.Compression.ZipFile]::CreateFromDirectory($OutDir, $ZipPath)

Write-Host "Done! Folder:`n$OutDir"
Write-Host "ZIP:`n$ZipPath"
