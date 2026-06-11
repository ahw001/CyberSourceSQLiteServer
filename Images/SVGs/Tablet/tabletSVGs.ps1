<#
Build-MobileFirst-Tab11.ps1
Generates a fictional 11" tablet series (MobileFirst Tab 11) SVG pack and zips it:
- 16 front variants (4 colorways × 4 sensors) with UI
- 4 back variants
- 1 hero (angled)
- 1 exploded
- 2 clean (no-UI)
- README + brand-colors.json

Outputs:
- .\MobileFirst_Tab11_SVG_Variants\
- .\MobileFirst_Tab11_Variants.zip
#>

$OutDir = Join-Path (Get-Location) "MobileFirst_Tab11_SVG_Variants"
if (-not (Test-Path $OutDir)) { New-Item -ItemType Directory -Path $OutDir | Out-Null }

# Global toggle: when $true, all SVGs export on a transparent canvas (no page background rects)
$TransparentOutput = $true

# Brand config you can tweak
$Brand = @{
  brand_name = "MobileFirst"
  series_name = "Tab 11"
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
  <radialGradient id="brandGlow" cx="50%" cy="40%" r="65%">
    <stop offset="0%" stop-color="var(--accent)" stop-opacity=".22"/>
    <stop offset="100%" stop-color="var(--accent)" stop-opacity="0"/>
  </radialGradient>
"@

function Get-TabletFront([string]$Sensor, [bool]$IncludeUI=$true) {
  $sensorSvg = switch ($Sensor) {
    "pill"     { '<rect x="600" y="100" width="90" height="18" rx="9" class="sensorFill" opacity=".78"/>' }
    "hole"     { '<circle cx="645" cy="109" r="10" class="sensorFill" opacity=".88"/>' }
    "teardrop" { '<path d="M645 96 c11 11 11 28 0 39 a19 19 0 1 1 -0.1 -39z" class="sensorFill" opacity=".84"/>' }
    default    { '' }
  }

  $ui = if ($IncludeUI) {
@'
        <rect x="50" y="50" rx="90" ry="90" width="1100" height="1700" fill="url(#brandGlow)"/>
        <g transform="translate(180, 760)" class="uiText hiContrast">
          <text x="0" y="0" font-size="140" font-weight="800" fill="var(--text)">MobileFirst</text>
          <text x="0" y="150" font-size="96" font-weight="700" fill="var(--text)">Tab 11</text>
          <text x="0" y="270" font-size="48" font-weight="500" fill="var(--text)" opacity=".85">11&quot; edge-to-edge • 120Hz • Slim bezels</text>
        </g>
        <g transform="translate(460, 1220)" class="uiText hiContrast">
          <rect x="0" y="0" rx="32" ry="32" width="620" height="108" fill="rgba(0,0,0,0.28)" stroke="rgba(255,255,255,0.18)"/>
          <text x="310" y="72" text-anchor="middle" font-size="46" font-weight="700" fill="var(--text)">Preorder</text>
        </g>
'@
  } else { "" }

@"
    <g filter="url(#drop)">
      <rect x="0" y="0" rx="96" ry="96" width="1200" height="1800" fill="url(#gradFrame)"/>
      <rect x="36" y="36" rx="84" ry="84" width="1128" height="1728" fill="var(--bezel)"/>
      <rect x="50" y="50" rx="78" ry="78" width="1100" height="1700" fill="url(#gradGlass)" filter="url(#innerShadow)"/>
      <rect x="94" y="94" width="1012" height="170" rx="50" fill="url(#reflect)"/>
      <path d="M140,280 C760,30 940,160 1200,0 L1200,0 L1200,1800 L140,1800 Z" fill="url(#gloss)" opacity=".36"/>
      <rect x="-10" y="520" width="12" height="190" rx="6" class="btnFill"/>
      <rect x="-10" y="740" width="12" height="190" rx="6" class="btnFill"/>
      <rect x="1198" y="640" width="12" height="240" rx="6" class="btnFill"/>
      $sensorSvg
    </g>
    $ui
"@
}

function Get-TabletBack() {
@'
    <g filter="url(#drop)">
      <rect x="0" y="0" rx="96" ry="96" width="1200" height="1800" fill="url(#gradFrame)"/>
      <rect x="20" y="20" rx="88" ry="88" width="1160" height="1760" fill="rgba(255,255,255,0.03)"/>
      <g transform="translate(260,140)">
        <rect x="0" y="0" rx="26" ry="26" width="680" height="120" fill="#14171b" opacity=".92"/>
        <circle cx="80" cy="60" r="40" fill="#0c0f12" stroke="#1f2329" stroke-width="6"/>
        <circle cx="200" cy="60" r="22" fill="#0c0f12" stroke="#1f2329" stroke-width="5"/>
        <circle cx="600" cy="60" r="14" fill="#2a2f36"/>
      </g>
      <rect x="-10" y="520" width="12" height="190" rx="6" class="btnFill"/>
      <rect x="-10" y="740" width="12" height="190" rx="6" class="btnFill"/>
      <rect x="1198" y="640" width="12" height="240" rx="6" class="btnFill"/>
    </g>
    <g class="uiText hiContrast" transform="translate(600, 980)">
      <text text-anchor="middle" font-size="78" fill="var(--text)">MobileFirst</text>
      <text y="88" text-anchor="middle" font-size="48" fill="var(--text)" opacity=".9">Tab 11</text>
    </g>
'@
}

function Wrap-SVG([string]$Inner, [hashtable]$CW, [string]$Title, [Nullable[bool]]$IncludeBG = $null) {
  $style = Get-StyleBlock

  if ($null -eq $IncludeBG) {
    $include = -not $TransparentOutput
  } else {
    $include = $IncludeBG
  }

  $bg = if ($include) { '<rect width="1400" height="2400" fill="var(--bg)"/>' } else { '' }

  # Put CSS variables directly on <svg> (single line)
  $svgVars = ("--frameTop:{0}; --frameSide:{1}; --frameEdge:{2}; --bezel:{3}; --glassTop:{4}; --glassBottom:{5}; --bg:{6}; --text:{7}; --accent:{8};" -f `
    $CW.frameTop, $CW.frameSide, $CW.frameEdge, $CW.bezel, $CW.glassTop, $CW.glassBottom, $CW.bg, $CW.text, $Brand.accent)

@"
<?xml version="1.0" encoding="UTF-8"?>
<!-- $Title | Generated $(Get-Date -Format yyyy-MM-dd) -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 2400" width="1400" height="2400" style="$svgVars">
  $style
  <defs>$DefsBlock</defs>
  $bg
  <g transform="translate(100,300) scale(1.0)">
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
    $title = "Tablet Front • $($cwName.ToUpperInvariant()) • Sensor: $sensorName"
    $inner = Get-TabletFront -Sensor $s -IncludeUI $true
    # Omit -IncludeBG so Wrap-SVG uses global $TransparentOutput
    $svg = Wrap-SVG -Inner $inner -CW $cw -Title $title
    $fname = "mobilefirst_tab11_front_{0}_{1}.svg" -f $cwName, $sensorName
    $path = Join-Path $OutDir $fname
    Write-Text $path $svg
    $created += $path
  }
}

# Back (4)
foreach ($cwName in $cws) {
  $cw = $Brand.colorways.$cwName
  $title = "Tablet Back • $($cwName.ToUpperInvariant())"
  $inner = Get-TabletBack
  $svg = Wrap-SVG -Inner $inner -CW $cw -Title $title
  $path = Join-Path $OutDir ("mobilefirst_tab11_back_{0}.svg" -f $cwName)
  Write-Text $path $svg
  $created += $path
}

# Helper: build inline CSS var block for manual SVGs
function Get-SvgVars([hashtable]$cw, [string]$accent) {
  ("--frameTop:{0}; --frameSide:{1}; --frameEdge:{2}; --bezel:{3}; --glassTop:{4}; --glassBottom:{5}; --bg:{6}; --text:{7}; --accent:{8};" -f `
    $cw.frameTop, $cw.frameSide, $cw.frameEdge, $cw.bezel, $cw.glassTop, $cw.glassBottom, $cw.bg, $cw.text, $accent)
}

# Hero (titanium, pill) — manual SVG, so include variables on <svg>
$cw = $Brand.colorways.titanium
$styleHero = Get-StyleBlock
$svgVarsHero = Get-SvgVars $cw $Brand.accent
$hero = @"
<?xml version="1.0" encoding="UTF-8"?>
<!-- Tablet Hero • Titanium • Pill -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 2400" width="1400" height="2400" style="$svgVarsHero">
  $styleHero
  <defs>$DefsBlock</defs>
  $(if(-not $TransparentOutput){ '<rect width="1400" height="2400" fill="var(--bg)"/>' } else { '' })
  <g opacity=".42" transform="translate(360,420) rotate(-9) scale(0.9) translate(-100,-300)">$(Get-TabletBack)</g>
  <g transform="translate(720,260) skewX(-6) rotate(10) scale(1.02) translate(-100,-300)">$(Get-TabletFront -Sensor 'pill' -IncludeUI $true)</g>
</svg>
"@
$heroPath = Join-Path $OutDir "mobilefirst_tab11_hero_titanium_pill.svg"
Write-Text $heroPath $hero
$created += $heroPath

# Exploded (black, hole) — manual SVG, include variables on <svg>
$cw = $Brand.colorways.black
$styleExpl = Get-StyleBlock
$svgVarsExpl = Get-SvgVars $cw $Brand.accent
$expl = @"
<?xml version="1.0" encoding="UTF-8"?>
<!-- Tablet Exploded • Black • Hole -->
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1400 2400" width="1400" height="2400" style="$svgVarsExpl">
  $styleExpl
  <defs>$DefsBlock</defs>
  $(if(-not $TransparentOutput){ '<rect width="1400" height="2400" fill="var(--bg)"/>' } else { '' })
  <g transform="translate(100,300)">$(Get-TabletFront -Sensor $null -IncludeUI $false)</g>
  <g transform="translate(100,200)"><rect x="50" y="50" rx="78" ry="78" width="1100" height="1700" fill="url(#gradGlass)" filter="url(#innerShadow)"/></g>
  <g transform="translate(100,160)"><circle cx="645" cy="109" r="10" class="sensorFill" opacity=".88"/></g>
  <g transform="translate(100,120)">
    <rect x="50" y="50" rx="90" ry="90" width="1100" height="1700" fill="url(#brandGlow)"/>
    <g transform="translate(180, 760)" class="uiText hiContrast">
      <text x="0" y="0" font-size="140" font-weight="800" fill="var(--text)">MobileFirst</text>
      <text x="0" y="150" font-size="96" font-weight="700" fill="var(--text)">Tab 11</text>
    </g>
  </g>
</svg>
"@
$explPath = Join-Path $OutDir "mobilefirst_tab11_exploded_black_hole.svg"
Write-Text $explPath $expl
$created += $explPath

# Clean: front blue (hole), back gold
$cw = $Brand.colorways.blue
$inner = Get-TabletFront -Sensor "hole" -IncludeUI $false
$svg = Wrap-SVG -Inner $inner -CW $cw -Title "Tablet Clean • Blue • Front"
$cleanFront = Join-Path $OutDir "mobilefirst_tab11_clean_front_blue_hole.svg"
Write-Text $cleanFront $svg
$created += $cleanFront

$cw = $Brand.colorways.gold
$inner = Get-TabletBack
$svg = Wrap-SVG -Inner $inner -CW $cw -Title "Tablet Clean • Gold • Back"
$cleanBack = Join-Path $OutDir "mobilefirst_tab11_clean_back_gold.svg"
Write-Text $cleanBack $svg
$created += $cleanBack

# README + brand-colors.json
$readme = @"
MobileFirst Tab 11 — Separate SVG Variant Pack
Generated on $(Get-Date -Format yyyy-MM-dd)

Contents:
- 16 front variants (4 colorways × 4 sensor styles) with UI
- 4 back variants
- 1 hero (angled)
- 1 exploded
- 2 clean variants (no UI)

Editing:
- Each SVG exposes CSS vars on the root <svg> element:
  --frameTop, --frameSide, --frameEdge, --bezel, --glassTop, --glassBottom, --bg, --text, --accent
- Typography via the .uiText font stack. High-contrast text via .hiContrast.

Notes:
- Fictional 11" tablet illustration; avoids replicating specific brand trade dress.
- Vector-based; scale to any resolution.
"@
$readme | Out-File (Join-Path $OutDir "README.txt") -Encoding UTF8

($Brand | ConvertTo-Json -Depth 6) | Out-File (Join-Path $OutDir "brand-colors.json") -Encoding UTF8

# ZIP everything
Add-Type -AssemblyName System.IO.Compression.FileSystem
$ZipPath = Join-Path (Get-Location) "MobileFirst_Tab11_Variants.zip"
if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }
[System.IO.Compression.ZipFile]::CreateFromDirectory($OutDir, $ZipPath)

Write-Host "Done! Folder:`n$OutDir"
Write-Host "ZIP:`n$ZipPath"
