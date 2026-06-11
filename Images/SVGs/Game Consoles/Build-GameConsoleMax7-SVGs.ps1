param(
  [string]$OutDir = ".\out"
)

if (-not (Test-Path -LiteralPath $OutDir)) {
  New-Item -ItemType Directory -Path $OutDir | Out-Null
}

function Get-ContrastColor {
  param([string]$Hex)
  $clean = $Hex.Trim().TrimStart('#')
  if ($clean.Length -ne 6) { return "#0b0d10" }
  $r = [Convert]::ToInt32($clean.Substring(0,2),16) / 255.0
  $g = [Convert]::ToInt32($clean.Substring(2,2),16) / 255.0
  $b = [Convert]::ToInt32($clean.Substring(4,2),16) / 255.0
  function _lin([double]$c){ if($c -le 0.03928){$c/12.92}else{[math]::Pow(($c+0.055)/1.055,2.4)} }
  $L = 0.2126*(_lin $r)+0.7152*(_lin $g)+0.0722*(_lin $b)
  if ($L -lt 0.45) { "#ffffff" } else { "#0b0d10" }
}

$themes = @(
  @{ Name="white"; Shell="#f4f6f9"; Edge="#cfd5dd"; Vent="#0e1116"; Accent="#4db2ff"; Port="#9aa3b2" },
  @{ Name="black"; Shell="#14181f"; Edge="#2a303a"; Vent="#000000"; Accent="#7ed3ff"; Port="#8a93a3" },
  @{ Name="silver";Shell="#d9dde3"; Edge="#b9c0ca"; Vent="#111419"; Accent="#4db2ff"; Port="#a5adba" },
  @{ Name="blue";  Shell="#1f3d88"; Edge="#2c4ca3"; Vent="#0a0f19"; Accent="#7ed3ff"; Port="#b4c2d8" }
)

function New-GCM7XboxFrontSvg {
  param([hashtable]$T)
  $text = Get-ContrastColor $T.Shell
  $label = "Front view — " + ($T.Name.Substring(0,1).ToUpper()+$T.Name.Substring(1))

  $svg = @'
<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 1100 1600" role="img" aria-labelledby="title desc">
  <title id="title">GameConsoleMax 7 — Front</title>
  <desc id="desc">Generic Xbox-style tower front view, transparent background.</desc>
'@

  $svg += @"
  <style>
    :root{
      --shell: $($T.Shell);
      --edge: $($T.Edge);
      --vent: $($T.Vent);
      --accent: $($T.Accent);
      --port: $($T.Port);
      --text: $text;
      --shadow: rgba(0,0,0,.25);
    }
    .tower { fill: var(--shell); stroke: var(--edge); stroke-width: 10 }
    .bevel { fill: none; stroke: rgba(0,0,0,.08); stroke-width: 8 }
    .slot  { fill: #12151a }
    .power { fill: none; stroke: var(--text); stroke-width: 12 }
    .usb   { fill: var(--port) }
    .dot   { fill: var(--vent) }
    .led   { fill: var(--accent) }
    .logo1 { font: 900 70px 'Segoe UI', Roboto, system-ui, sans-serif; fill: var(--text); letter-spacing: 1px; text-anchor: middle }
    .logo2 { font: 800 56px 'Segoe UI', Roboto, system-ui, sans-serif; fill: var(--accent); text-anchor: middle; filter: url(#glow) }
    text   { font: 700 44px system-ui, -apple-system, Segoe UI, Roboto, Arial; fill: var(--text) }
    .small { font: 500 24px system-ui, -apple-system, Segoe UI, Roboto, Arial; fill: var(--text) }
    .shadow{ fill: var(--shadow) }
  </style>

  <!-- glow filter for accent logo -->
  <defs>
    <filter id="glow" x="-50%" y="-50%" width="200%" height="200%">
      <feGaussianBlur in="SourceGraphic" stdDeviation="3.5" result="b"/>
      <feMerge><feMergeNode in="b"/><feMergeNode in="SourceGraphic"/></feMerge>
    </filter>
  </defs>

  <!-- ground shadow -->
  <ellipse class="shadow" cx="550" cy="1500" rx="360" ry="45" opacity=".45"/>

  <!-- console body -->
  <rect class="tower" x="250" y="140" width="600" height="1220" rx="36"/>
  <rect class="bevel" x="270" y="160" width="560" height="1180" rx="30"/>

  <!-- vent grid -->
  <g transform="translate(290,200)">
"@

  for ($r=0;$r -lt 6;$r++) {
    for ($c=0;$c -lt 8;$c++) {
      $x = 20 + $c*60
      $y = 20 + $r*60
      $svg += "    <circle class='dot' cx='$x' cy='$y' r='8' />`n"
    }
  }

  $svg += @"
  </g>

  <!-- slot, ports, button -->
  <rect class='slot' x='760' y='420' width='10' height='540' rx='5' opacity='.75'/>
  <circle class='power' cx='330' cy='300' r='24'/>
  <circle class='led' cx='330' cy='300' r='4'/>
  <g transform='translate(350,1120)'>
    <rect class='usb' x='0' y='0' width='110' height='22' rx='6'/>
    <rect class='usb' x='150' y='0' width='110' height='22' rx='6'/>
  </g>

  <!-- front logo (two lines, second glows in accent color) -->
  <text class='logo1' x='550' y='900'>GameConsole</text>
  <text class='logo2' x='550' y='980'>Max 7</text>

  <!-- footer labels -->
  <text x='250' y='1400'>GameConsoleMax 7</text>
  <text class='small' x='250' y='1440'>$label</text>
</svg>
"@

  return $svg
}

foreach ($t in $themes) {
  $svg = New-GCM7XboxFrontSvg -T $t
  $file = Join-Path $OutDir ("gameconsolemax7_front_{0}.svg" -f $t.Name)
  $svg | Set-Content -LiteralPath $file -Encoding UTF8
  Write-Host "Wrote $file"
}
Write-Host "Done. SVGs saved to $OutDir"
