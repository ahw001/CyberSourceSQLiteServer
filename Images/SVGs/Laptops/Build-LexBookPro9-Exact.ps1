<#
Build-LexBookPro9-Exact.ps1
Generates four front-view SVGs of a MacBook-style laptop (LexBook Pro 9).
- Realistic proportions: thin uniform bezel, central notch, rounded corners
- Hinge/lip bar under the display (subtle shadow)
- Transparent background
- Screensaver text: "LexBook" (top line), "Pro 9" (bottom line)

Outputs:
  .\LexBook_Pro9_SVGs\lexbook_pro9_front_<color>.svg
#>

# --- Output folder ------------------------------------------------------------
$OutDir = Join-Path (Get-Location) "LexBook_Pro9_SVGs"
if (-not (Test-Path $OutDir)) { New-Item -ItemType Directory -Path $OutDir | Out-Null }

# --- Canvas & geometry --------------------------------------------------------
# Generous canvas with padding to avoid any clipping
$W = 2400     # width
$H = 1600     # height
$Pad = 24     # safe padding all around the lid

# Lid (display housing) — rounded rectangle
# Aspect visually centered, similar to MBP's head-on look
$LidWidth  = 1920
$LidHeight = 1180
$LidRx = 34
$LidX = ($W - $LidWidth) / 2
$LidY = 140

# Bezel thickness (uniform around screen)
$Bezel = 26

# Screen (inside bezel)
$ScreenX = $LidX + $Bezel
$ScreenY = $LidY + $Bezel
$ScreenW = $LidWidth - 2*$Bezel
$ScreenH = $LidHeight - 2*$Bezel

# Notch (centered) — width/height tuned to resemble MBP 14/16 notch
$NotchW = 190
$NotchH = 58
$NotchRx = 14
$NotchX = $LidX + ($LidWidth - $NotchW)/2
$NotchY = $LidY

# Camera dot inside notch (just for realism)
$CamR = 7
$CamCx = $NotchX + $NotchW*0.32
$CamCy = $NotchY + $NotchH*0.52

# Hinge/lip bar below the lid (very thin base showing)
$LipW = $LidWidth + 380
$LipH = 36
$LipRx = 14
$LipX = ($W - $LipW)/2
$LipY = $LidY + $LidHeight + 18

# Trackpad cutout hint in the lip (centered shallow arc)
$DipW = 260
$DipH = 18
$DipRx = 9
$DipX = $LipX + ($LipW - $DipW)/2
$DipY = $LipY + 6

# Font
$FontStack = "system-ui, -apple-system, 'Segoe UI', Roboto, Ubuntu, Cantarell, 'Helvetica Neue', Arial, sans-serif"

# --- Colorways (lid/bezel/lip variations) ------------------------------------
$Colorways = @(
  @{ name='white';  lid='#F6F7FA'; rim='#E7E9EE'; bezel='#0B0D10'; lip='#CFD3DA'; lipEdge='#B7BCC6' },
  @{ name='black';  lid='#1A1C1F'; rim='#2A2D31'; bezel='#050608'; lip='#2B2E33'; lipEdge='#3B3F45'  },
  @{ name='silver'; lid='#D8DCE3'; rim='#C9CED8'; bezel='#0C0E12'; lip='#C6CCD6'; lipEdge='#B5BCC8' },
  @{ name='blue';   lid='#2C3F62'; rim='#37517C'; bezel='#0C1016'; lip='#324A71'; lipEdge='#415C88' }
)

# --- Screen gradients & shadows ----------------------------------------------
function Get-ScreenDefs {
  param([string]$idGrad, [string]$idVignette)
@"
  <defs>
    <!-- Vibrant diagonal gradient similar to macOS wallpapers -->
    <linearGradient id='$idGrad' x1='0%' y1='0%' x2='100%' y2='100%'>
      <stop offset='0%'   stop-color='#5E0CEB'/>
      <stop offset='45%'  stop-color='#C20E4D'/>
      <stop offset='75%'  stop-color='#FF6A00'/>
      <stop offset='100%' stop-color='#00C2FF'/>
    </linearGradient>

    <!-- Soft vignette over the screen -->
    <radialGradient id='$idVignette' cx='50%' cy='50%' r='70%'>
      <stop offset='0%' stop-color='black' stop-opacity='0'/>
      <stop offset='100%' stop-color='black' stop-opacity='0.25'/>
    </radialGradient>

    <!-- Drop-shadow for lid -->
    <filter id='lidShadow' x='-20%' y='-20%' width='140%' height='140%'>
      <feDropShadow dx='0' dy='10' stdDeviation='12' flood-color='black' flood-opacity='0.18'/>
    </filter>
  </defs>
"@
}

# --- Build a single SVG -------------------------------------------------------
function New-LexBookSvg {
  param([hashtable]$cw)

  $gradId = "scr_$($cw.name)"
  $vigId  = "vig_$($cw.name)"
  $defs = Get-ScreenDefs -idGrad $gradId -idVignette $vigId

  # A tiny inner rim to mimic the anodized edge around the lid
  $InnerOffset = 6

  $svg = @"
<svg xmlns='http://www.w3.org/2000/svg' width='$W' height='$H' viewBox='0 0 $W $H'>
  $defs

  <!-- Lid outer -->
  <g filter='url(#lidShadow)'>
    <rect x='$LidX' y='$LidY' width='$LidWidth' height='$LidHeight' rx='$LidRx' fill='$($cw.rim)'/>
    <rect x='$( $LidX + $InnerOffset )' y='$( $LidY + $InnerOffset )'
          width='$( $LidWidth - 2*$InnerOffset )' height='$( $LidHeight - 2*$InnerOffset )'
          rx='$( $LidRx - $InnerOffset )' fill='$($cw.lid)'/>
  </g>

  <!-- Uniform bezel -->
  <rect x='$LidX' y='$LidY' width='$LidWidth' height='$LidHeight' rx='$LidRx'
        fill='none' stroke='$($cw.bezel)' stroke-width='$( 2*$Bezel )'/>

  <!-- Notch (cut into bezel) -->
  <rect x='$NotchX' y='$NotchY' width='$NotchW' height='$NotchH' rx='$NotchRx' fill='$($cw.bezel)'/>

  <!-- Camera dot -->
  <circle cx='$CamCx' cy='$CamCy' r='$CamR' fill='#1A1D22' />

  <!-- Screen panel -->
  <g>
    <rect x='$ScreenX' y='$ScreenY' width='$ScreenW' height='$ScreenH' rx='16' fill='url(#$gradId)'/>
    <rect x='$ScreenX' y='$ScreenY' width='$ScreenW' height='$ScreenH' rx='16' fill='url(#$vigId)'/>
    <!-- Screensaver text -->
    <g font-family="$FontStack" text-anchor='middle' fill='white'>
      <text x='$( $ScreenX + $ScreenW/2 )' y='$( $ScreenY + $ScreenH*0.46 )'
            font-size='150' font-weight='700' letter-spacing='1'>LexBook</text>
      <text x='$( $ScreenX + $ScreenW/2 )' y='$( $ScreenY + $ScreenH*0.61 )'
            font-size='120' font-weight='600' opacity='0.95'>Pro 9</text>
    </g>
  </g>

  <!-- Hinge / lip bar -->
  <g>
    <rect x='$LipX' y='$LipY' width='$LipW' height='$LipH' rx='$LipRx' fill='$($cw.lip)'
          stroke='$($cw.lipEdge)' stroke-width='2'/>
    <!-- small center dip -->
    <rect x='$DipX' y='$DipY' width='$DipW' height='$DipH' rx='$DipRx' fill='#000' opacity='0.15'/>
  </g>
</svg>
"@

  $svg
}

# --- Render all colorways -----------------------------------------------------
foreach ($cw in $Colorways) {
  $svg = New-LexBookSvg -cw $cw
  $file = Join-Path $OutDir ("lexbook_pro9_front_{0}.svg" -f $cw.name)
  Set-Content -Path $file -Value $svg -Encoding UTF8
}

Write-Host "Done! SVGs created in: $OutDir"
