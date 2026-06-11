<#
Build-XSXMax9-HeadphonesSVGs.ps1
Generates compact hi-res transparent SVGs (front view) for a fictional headphone: "XSX Max 9".
Colorways: White, Black, Silver, Blue.
Includes a visible volume adjuster with contrast.
No text appears on the device; name exists only in comments.
#>

$OutDir = Join-Path (Get-Location) "XSXMax9_Headphones_SVG"
if (-not (Test-Path $OutDir)) { New-Item -ItemType Directory -Path $OutDir | Out-Null }

# Smaller canvas
$W = 800
$H = 800
$ViewBox = "0 0 $W $H"

# Geometry scaled down proportionally
$bandOuterR = 320
$bandThickness = 40
$earWidth  = 204
$earHeight = 248
$earRadius = 96
$earGapFromCenter = 168
$earTop = 260
$yokeWidth = 36
$yokeHeight = 88
$yokeRadius = 18
$cushionInset = 18
$padInset = 48
$grillInset = 84
$grillRadius = 8
$volXOffsetFromRightEarEdge = 22
$volTopOffset = 72
$volBottomOffset = 176
$volTrackWidth = 6
$volKnobR = 12

$Colorways = @(
  @{ name="white"; body="#f7f8fa"; bandShadow="#d7dbe3"; metal="#e3e6ec";
     cushion="#e9ebf0"; pad="#cfd4dc"; grill="#aeb7c4"; stroke="#8893a3";
     volumeAccent="#2c7cff" },
  @{ name="black"; body="#1a1c20"; bandShadow="#0f1114"; metal="#2a2d33";
     cushion="#202329"; pad="#3a3f48"; grill="#596473"; stroke="#8793a3";
     volumeAccent="#e6b800" },
  @{ name="silver"; body="#cfd5dd"; bandShadow="#b8c0cb"; metal="#dde2e8";
     cushion="#e7ebf1"; pad="#c1c9d3"; grill="#9aa6b7"; stroke="#798597";
     volumeAccent="#2f9e44" },
  @{ name="blue"; body="#2a61c9"; bandShadow="#224fa3"; metal="#345bd3";
     cushion="#274ca0"; pad="#5a86e0"; grill="#9eb0e8"; stroke="#c7d2ff";
     volumeAccent="#ff4d4f" }
)

function New-SVG-Headphones {
  param([hashtable]$Theme, [string]$Path)

  $leftEarX  = 400 - $earGapFromCenter - ($earWidth/2)
  $rightEarX = 400 + $earGapFromCenter - ($earWidth/2)
  $bandCX = 400; $bandCY = 460; $R = $bandOuterR - ($bandThickness/2)
  $leftBandX = 400 - $earGapFromCenter; $rightBandX = 400 + $earGapFromCenter
# Leave a bit more headroom so the band stroke doesn't clip
$bandY = $earTop - 8   # was: $earTop - 24

# Draw the *small* top arc: large-arc-flag = 0, sweep-flag = 1
$bandPath = "M $leftBandX $bandY A $R $R 0 0 1 $rightBandX $bandY"

  function RRect([double]$x,[double]$y,[double]$w,[double]$h,[double]$r){
    $r=[math]::Min($r,[math]::Min($w/2,$h/2))
    "M $($x+$r) $y H $($x+$w-$r) A $r $r 0 0 1 $($x+$w) $($y+$r) V $($y+$h-$r) " +
    "A $r $r 0 0 1 $($x+$w-$r) $($y+$h) H $($x+$r) A $r $r 0 0 1 $x $($y+$h-$r) " +
    "V $($y+$r) A $r $r 0 0 1 $($x+$r) $y Z"
  }

  $lOuter = RRect $leftEarX $earTop $earWidth $earHeight $earRadius
  $rOuter = RRect $rightEarX $earTop $earWidth $earHeight $earRadius
  $lCush = RRect ($leftEarX+$cushionInset) ($earTop+$cushionInset) ($earWidth-2*$cushionInset) ($earHeight-2*$cushionInset) ($earRadius-($cushionInset*0.6))
  $rCush = RRect ($rightEarX+$cushionInset) ($earTop+$cushionInset) ($earWidth-2*$cushionInset) ($earHeight-2*$cushionInset) ($earRadius-($cushionInset*0.6))
  $lPad  = RRect ($leftEarX+$padInset) ($earTop+$padInset) ($earWidth-2*$padInset) ($earHeight-2*$padInset) ($earRadius-($padInset*0.6))
  $rPad  = RRect ($rightEarX+$padInset) ($earTop+$padInset) ($earWidth-2*$padInset) ($earHeight-2*$padInset) ($earRadius-($padInset*0.6))
  $lGrill= RRect ($leftEarX+$grillInset) ($earTop+$grillInset) ($earWidth-2*$grillInset) ($earHeight-2*$grillInset) $grillRadius
  $rGrill= RRect ($rightEarX+$grillInset) ($earTop+$grillInset) ($earWidth-2*$grillInset) ($earHeight-2*$grillInset) $grillRadius
  $leftYokeX  = $leftBandX - ($yokeWidth/2)
  $rightYokeX = $rightBandX - ($yokeWidth/2)
  $yokeY = $earTop - 12
  $leftYoke  = RRect $leftYokeX  $yokeY $yokeWidth $yokeHeight $yokeRadius
  $rightYoke = RRect $rightYokeX $yokeY $yokeWidth $yokeHeight $yokeRadius
  $rightEdgeX = $rightEarX + $earWidth
  $volTrackX = $rightEdgeX - $volXOffsetFromRightEarEdge
  $volTrackY1 = $earTop + $volTopOffset
  $volTrackY2 = $earTop + $volBottomOffset
  $volKnobY = ($volTrackY1 + $volTrackY2)/2

  $svg = @"
<?xml version="1.0" encoding="UTF-8"?>
<!-- XSX Max 9 - Compact SVG | Colorway: $($Theme.name) -->
<svg xmlns="http://www.w3.org/2000/svg" width="$W" height="$H" viewBox="$ViewBox">
  <defs>
    <linearGradient id="bandGrad-$($Theme.name)" x1="0" y1="0" x2="0" y2="1">
      <stop offset="0%" stop-color="$($Theme.body)"/><stop offset="100%" stop-color="$($Theme.bandShadow)"/>
    </linearGradient>
    <linearGradient id="cupGrad-$($Theme.name)" x1="0" y1="0" x2="0" y2="1">
      <stop offset="0%" stop-color="$($Theme.body)"/><stop offset="100%" stop-color="$($Theme.metal)"/>
    </linearGradient>
    <linearGradient id="cushGrad-$($Theme.name)" x1="0" y1="0" x2="0" y2="1">
      <stop offset="0%" stop-color="$($Theme.cushion)"/><stop offset="100%" stop-color="$($Theme.pad)"/>
    </linearGradient>
  </defs>

  <path d="$bandPath" fill="none" stroke="url(#bandGrad-$($Theme.name))"
        stroke-width="$bandThickness" stroke-linecap="round"/>

  <path d="$leftYoke"  fill="url(#cupGrad-$($Theme.name))" stroke="$($Theme.stroke)" stroke-width="3"/>
  <path d="$rightYoke" fill="url(#cupGrad-$($Theme.name))" stroke="$($Theme.stroke)" stroke-width="3"/>

  <path d="$lOuter"  fill="url(#cupGrad-$($Theme.name))" stroke="$($Theme.stroke)" stroke-width="4"/>
  <path d="$rOuter"  fill="url(#cupGrad-$($Theme.name))" stroke="$($Theme.stroke)" stroke-width="4"/>
  <path d="$lCush"  fill="url(#cushGrad-$($Theme.name))" stroke="$($Theme.stroke)" stroke-width="2"/>
  <path d="$rCush"  fill="url(#cushGrad-$($Theme.name))" stroke="$($Theme.stroke)" stroke-width="2"/>
  <path d="$lPad"  fill="$($Theme.pad)" opacity="0.85"/>
  <path d="$rPad"  fill="$($Theme.pad)" opacity="0.85"/>
  <path d="$lGrill" fill="$($Theme.grill)" opacity="0.3"/>
  <path d="$rGrill" fill="$($Theme.grill)" opacity="0.3"/>

  <rect x="$([math]::Round($volTrackX - $volTrackWidth/2,2))" y="$([math]::Round($volTrackY1,2))"
        width="$volTrackWidth" height="$([math]::Round($volTrackY2 - $volTrackY1,2))"
        rx="3" ry="3" fill="$($Theme.volumeAccent)" opacity="0.25"
        stroke="$($Theme.volumeAccent)" stroke-width="2" />
  <circle cx="$volTrackX" cy="$([math]::Round($volKnobY,2))" r="$volKnobR"
          fill="$($Theme.volumeAccent)" stroke="white" stroke-width="2" />
</svg>
"@
  [System.IO.File]::WriteAllText($Path, $svg, [System.Text.Encoding]::UTF8)
}

foreach ($cw in $Colorways) {
  $path = Join-Path $OutDir ("xsxmax9_headphones_front_{0}.svg" -f $cw.name)
  New-SVG-Headphones -Theme $cw -Path $path
}

Write-Host "Generated compact SVGs in: $OutDir" -ForegroundColor Green
