<#
  SmartWatch Pro 5 — SVG builder (Front view + clock + optional complications)
  - Four transparent SVGs (front view only): white, black, silver, blue.
  - No visible brand text on the device (name is comment-only).
  - Analog clock set to ~10:10:36 with hour/minute/second hands and tick marks.

  Update:
  - Complications moved to watch-face corners:
      * Date (Top-Left)
      * Battery ring + % (Top-Right)
      * Steps mini-dial + count (Bottom-Left)
	  
  Command with options: ./Build-SmartWatchPro5-SVGs.ps1 -ShowDate -ShowBattery -ShowSteps
#>

param(
  [string]$OutDir = ".\out",

  # Complication toggles
  [switch]$ShowDate,
  [switch]$ShowBattery,
  [switch]$ShowSteps,

  # Complication values
  [ValidateRange(0,100)][int]$BatteryPercent = 76,
  [ValidateRange(1,31)][int]$DateDay = ([int](Get-Date).ToString('dd')),
  [int]$StepCount = 8250
)

if (-not (Test-Path -LiteralPath $OutDir)) {
  New-Item -ItemType Directory -Path $OutDir | Out-Null
}

# Theme presets (white/silver clock styling matches black/blue)
$themes = @(
  @{ Name="white"; Case="#f8f8f8"; Edge="#d0d3d8"; Screen="#0b0e14"; Bezel="#161b22"; Band="#e6e8eb"; Crown="#c9cfd8"; Tick="#e9edf5"; Hand="#e9edf5"; Second="#7ed3ff"; UseMetal=$false },
  @{ Name="black"; Case="#1b212b"; Edge="#4a5260"; Screen="#05070a"; Bezel="#0c1118"; Band="#212833"; Crown="#616b7b"; Tick="#e9edf5"; Hand="#e9edf5"; Second="#7ed3ff"; UseMetal=$false },
  @{ Name="silver";Case="#d9dde3"; Edge="#b9c0ca"; Screen="#0e1116"; Bezel="#1a2029"; Band="#e9edf3"; Crown="#aab4c2"; Tick="#e9edf5"; Hand="#e9edf5"; Second="#7ed3ff"; UseMetal=$true; MetalStops=@("#eef1f5","#d9dde3","#cfd5dd","#d9dde3","#eef1f5") },
  @{ Name="blue";  Case="#1f3d88"; Edge="#2c4ca3"; Screen="#0e1522"; Bezel="#0f1728"; Band="#24479a"; Crown="#3358b6"; Tick="#e9edf5"; Hand="#e9edf5"; Second="#7ed3ff"; UseMetal=$false }
)

function New-SmartWatchClockSvg {
  param([hashtable]$T)

  # Optional brushed metal for silver
  $metalDef = ""
  $caseClass = "case"
  if ($T.UseMetal -and $T.ContainsKey("MetalStops")) {
    $s = $T.MetalStops
    $metalDef = @"
    <linearGradient id="gMetal" x1="0" x2="1" y1="0" y2="0">
      <stop offset="0"    stop-color="$($s[0])"/>
      <stop offset="0.25" stop-color="$($s[1])"/>
      <stop offset="0.5"  stop-color="$($s[2])"/>
      <stop offset="0.75" stop-color="$($s[3])"/>
      <stop offset="1"    stop-color="$($s[4])"/>
    </linearGradient>
"@
    $caseClass = "caseMetal"
  }

  # Layout constants (glass rect inside case)
  $glassX = 90;  $glassY = 120;  $glassW = 520;  $glassH = 660
  $cx = $glassX + [int]($glassW/2)   # 350
  $cy = $glassY + [int]($glassH/2)   # 450
  $rMajor = 240; $rMinor = 230

  # Classic 10:10:36 pose
  $angH = ([math]::PI/180) * ((10 + 10.0/60.0) * 30.0 - 90.0)
  $angM = ([math]::PI/180) * (10 * 6.0 - 90.0)
  $angS = ([math]::PI/180) * (36 * 6.0 - 90.0)
  $hLen = 140; $mLen = 190; $sLen = 210

  # Corner positions (with margin)
  $m = 22

  # Date (Top-Left)
  $dateW = 70; $dateH = 46
  $dateX = $glassX + $m
  $dateY = $glassY + $m
  $dateCX = [int]($dateX + $dateW/2); $dateCY = [int]($dateY + $dateH/2)

  # Battery ring (Top-Right)
  $battR = 40
  $battCX = $glassX + $glassW - $m - $battR
  $battCY = $glassY + $m + $battR
  $battSweep = [math]::Max(0,[math]::Min(100,$BatteryPercent)) * 3.6
  $bStart = -90

  # Steps mini-dial (Bottom-Left)
  $stepsR = 46
  $stepsCX = $glassX + $m + $stepsR
  $stepsCY = $glassY + $glassH - $m - $stepsR

  $svg = @"
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1200 1200" role="img">
  <defs>
    <linearGradient id="gScreen" x1="0" x2="0" y1="0" y2="1">
      <stop offset="0" stop-color="#1f252f"/>
      <stop offset="1" stop-color="$($T.Screen)"/>
    </linearGradient>
$metalDef  </defs>

  <style>
    :root{
      --case:  $($T.Case);   --edge:  $($T.Edge);
      --band:  $($T.Band);   --bezel: $($T.Bezel);
      --crown: $($T.Crown);  --tick:  $($T.Tick);
      --hand:  $($T.Hand);   --sec:   $($T.Second);
    }
    /* Transparent canvas: no background rect */

    /* Watch components */
    .band      { fill: var(--band) }
    .case      { fill: var(--case); stroke: var(--edge); stroke-width: 3 }
    .caseMetal { fill: url(#gMetal); stroke: var(--edge); stroke-width: 3 }
    .glass     { fill: url(#gScreen) }
    .bezel     { fill: none; stroke: var(--bezel); stroke-width: 10 }
    .crown     { fill: var(--crown) }

    /* Clock face */
    .tickH { stroke: var(--tick); stroke-width: 6; stroke-linecap: round }
    .tickM { stroke: var(--tick); stroke-width: 3; stroke-linecap: round; opacity: .85 }
    .handH { stroke: var(--hand); stroke-width: 12; stroke-linecap: round }
    .handM { stroke: var(--hand); stroke-width: 8;  stroke-linecap: round }
    .sec   { stroke: var(--sec);  stroke-width: 4;  stroke-linecap: round }
    .hub   { fill: var(--hand) }

    /* Complications */
    .dateBox { fill: #e8ecf3; stroke: #b7bfcd; stroke-width: 2; rx: 6; ry: 6 }
    .dateTxt { font: 700 28px 'Segoe UI', system-ui, sans-serif; fill: #0b0d10; text-anchor: middle; dominant-baseline: middle }
    .battBG  { fill: none; stroke: rgba(255,255,255,.18); stroke-width: 8 }
    .battFG  { fill: none; stroke: var(--sec); stroke-width: 8; stroke-linecap: round }
    .battTxt { font: 700 22px 'Segoe UI', system-ui, sans-serif; fill: var(--tick); text-anchor: middle; dominant-baseline: middle }
    .stepsBG { fill: none; stroke: rgba(255,255,255,.18); stroke-width: 6 }
    .stepsFG { fill: none; stroke: var(--tick); stroke-width: 6; stroke-linecap: round }
    .stepsTxt{ font: 700 22px 'Segoe UI', system-ui, sans-serif; fill: var(--tick); text-anchor: middle; dominant-baseline: middle }
  </style>

  <!-- FRONT VIEW (centered) -->
  <g transform="translate(250,150)">
    <!-- Band behind case -->
    <rect class="band" x="275" y="-40" width="100" height="880" rx="50"/>

    <!-- Case body -->
    <rect class="$caseClass" x="0" y="0" width="700" height="900" rx="180"/>

    <!-- Glass inset -->
    <rect class="glass" x="$glassX" y="$glassY" width="$glassW" height="$glassH" rx="150"/>

    <!-- Bezel outline -->
    <rect class="bezel" x="$glassX" y="$glassY" width="$glassW" height="$glassH" rx="150"/>

    <!-- Side crown -->
    <circle class="crown" cx="710" cy="450" r="18"/>
"@

  # 60 ticks
  for ($i=0; $i -lt 60; $i++) {
    $ang = [math]::PI * 2 * $i / 60
    $x1 = [math]::Round($cx + [math]::Cos($ang) * $rMinor, 3)
    $y1 = [math]::Round($cy + [math]::Sin($ang) * $rMinor, 3)
    $x2 = [math]::Round($cx + [math]::Cos($ang) * $rMajor, 3)
    $y2 = [math]::Round($cy + [math]::Sin($ang) * $rMajor, 3)
    if ($i % 5 -eq 0) {
      $x1h = [math]::Round($cx + [math]::Cos($ang) * ($rMinor - 10), 3)
      $y1h = [math]::Round($cy + [math]::Sin($ang) * ($rMinor - 10), 3)
      $svg += "    <line class='tickH' x1='$x1h' y1='$y1h' x2='$x2' y2='$y2' />`n"
    } else {
      $svg += "    <line class='tickM' x1='$x1' y1='$y1' x2='$x2' y2='$y2' />`n"
    }
  }

  # Hand endpoints
  $hx = [math]::Round($cx + [math]::Cos($angH) * $hLen, 3)
  $hy = [math]::Round($cy + [math]::Sin($angH) * $hLen, 3)
  $mx = [math]::Round($cx + [math]::Cos($angM) * $mLen, 3)
  $my = [math]::Round($cy + [math]::Sin($angM) * $mLen, 3)
  $sx = [math]::Round($cx + [math]::Cos($angS) * $sLen, 3)
  $sy = [math]::Round($cy + [math]::Sin($angS) * $sLen, 3)

  $svg += @"
    <!-- CLOCK FACE -->
    <line class='handH' x1='$cx' y1='$cy' x2='$hx' y2='$hy' />
    <line class='handM' x1='$cx' y1='$cy' x2='$mx' y2='$my' />
    <line class='sec'   x1='$cx' y1='$cy' x2='$sx' y2='$sy' />
    <circle class='hub' cx='$cx' cy='$cy' r='8'/>
"@

  # Date (Top-Left)
  if ($ShowDate) {
    $d = "{0:D2}" -f [math]::Max(1,[math]::Min(31,$DateDay))
    $svg += @"
    <!-- DATE (Top-Left) -->
    <rect class='dateBox' x='$dateX' y='$dateY' width='$dateW' height='$dateH' />
    <text class='dateTxt' x='$dateCX' y='$dateCY'>$d</text>
"@
  }

  # Battery (Top-Right)
  if ($ShowBattery) {
    function _toXY([double]$deg, [double]$r, [double]$ccx, [double]$ccy) {
      $rad = [math]::PI/180 * $deg
      $x = [math]::Round($ccx + [math]::Cos($rad)*$r, 3)
      $y = [math]::Round($ccy + [math]::Sin($rad)*$r, 3)
      return @($x,$y)
    }
    $endDeg = $bStart + $battSweep
    $start = _toXY $bStart $battR $battCX $battCY
    $end   = _toXY $endDeg $battR $battCX $battCY
    $largeArc = ([int]($battSweep -gt 180))
    $sweepFlag = 1
    $pathArc = "M $($start[0]) $($start[1]) A $battR $battR 0 $largeArc $sweepFlag $($end[0]) $($end[1])"
    $svg += @"
    <!-- BATTERY (Top-Right) -->
    <circle class='battBG' cx='$battCX' cy='$battCY' r='$battR'/>
    <path class='battFG' d='$pathArc'/>
    <text class='battTxt' x='$battCX' y='$([int]$battCY)'>$BatteryPercent%</text>
"@
  }

  # Steps (Bottom-Left)
  if ($ShowSteps) {
    $sArcR = $stepsR
    $sx1 = [math]::Round($stepsCX - $sArcR, 3)
    $sy1 = [math]::Round($stepsCY, 3)
    $sx2 = [math]::Round($stepsCX + $sArcR, 3)
    $sy2 = $sy1
    $svg += @"
    <!-- STEPS (Bottom-Left) -->
    <path class='stepsBG' d='M $sx1 $sy1 A $sArcR $sArcR 0 0 1 $sx2 $sy2'/>
    <line class='stepsFG' x1='$stepsCX' y1='$([int]($stepsCY - $sArcR))' x2='$stepsCX' y2='$stepsCY'/>
    <text class='stepsTxt' x='$stepsCX' y='$([int]($stepsCY + 26))'>$StepCount</text>
"@
  }

  $svg += @"
  </g>
</svg>
"@

  return $svg
}

# Generate per theme
foreach ($t in $themes) {
  $svg = New-SmartWatchClockSvg -T $t
  $file = Join-Path $OutDir ("smartwatchpro5_front_{0}.svg" -f $t.Name)
  $svg | Set-Content -LiteralPath $file -Encoding UTF8
  Write-Host "Wrote $file"
}

Write-Host "Done. SVGs saved to $OutDir"
