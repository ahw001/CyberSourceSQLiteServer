<# 
Generate-NorthwindCategorySVGs.ps1
Reads a CSV of categories and outputs 360x360 SVG icons per CategoryName.

- Input:  CSV content (inline here) or external file via -CsvPath.
- Output: ./CategorySVGs/<sanitized CategoryName>.svg
- Size:   360 x 360, transparent background
#>

[CmdletBinding()]
param(
  [string]$CsvPath
)

# --- Load CSV ---
if (-not $CsvPath) {
  $csvData = @"
CategoryId,CategoryName,Description,Picture
1,Beverages,"Soft drinks, coffees, teas, beers, and ales",
2,Condiments,"Sweet and savory sauces, relishes, spreads, and seasonings",
3,Confections,"Desserts, candies, and sweet breads",
4,Dairy Products,Cheeses,
5,Grains/Cereals,"Breads, crackers, pasta, and cereal",
6,Meat/Poultry,Prepared meats,
7,Produce,Dried fruit and bean curd,
8,Seafood,Seaweed and fish,
"@ | ConvertFrom-Csv
} else {
  if (-not (Test-Path $CsvPath)) { throw "CSV not found at: $CsvPath" }
  $csvData = Import-Csv -Path $CsvPath
}

# --- Output folder ---
$outDir = Join-Path (Get-Location) "CategorySVGs"
if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Path $outDir | Out-Null }

# --- Helpers ---
function Sanitize-FileName {
  param([string]$Name)
  $n = $Name.Trim()
  $invalid = ([IO.Path]::GetInvalidFileNameChars() + @('/','\')).ForEach({[regex]::Escape($_)}) -join '|'
  return ([regex]::Replace($n,$invalid,'_'))
}

# Color palette per category (feel free to tweak)
$palette = @{
  'Beverages'       = '#3B82F6' # blue
  'Condiments'      = '#F59E0B' # amber
  'Confections'     = '#EC4899' # pink
  'Dairy Products'  = '#22C55E' # green
  'Grains/Cereals'  = '#8B5CF6' # violet
  'Meat/Poultry'    = '#EF4444' # red
  'Produce'         = '#10B981' # emerald
  'Seafood'         = '#06B6D4' # cyan
}

# --- Core SVG builder ---
function New-CategorySvg {
  param(
    [pscustomobject]$Row,
    [string]$OutPath
  )

  $name        = $Row.CategoryName
  $desc        = $Row.Description
  $bg          = $palette[$name]
  if (-not $bg) { $bg = '#64748B' } # slate fallback

  # Shared sizes
  $w = 360; $h = 360
  $cx = 180; $cy = 180
  $badgeR = 140

  # Build glyph per category (white on colored circle)
  # Keep shapes simple & bold; all coordinates relative to 360x360 canvas
  $glyph = switch ($name) {

    'Beverages' {
      @"
      <!-- Cup with straw -->
      <rect x='125' y='140' width='110' height='120' rx='14' fill='white'/>
      <rect x='120' y='130' width='120' height='20' rx='10' fill='white'/>
      <rect x='200' y='90' width='10' height='50' rx='5' fill='white'/>
      "@
    }

    'Condiments' {
      @"
      <!-- Squeeze bottle -->
      <rect x='150' y='110' width='60' height='150' rx='20' fill='white'/>
      <rect x='160' y='90' width='40' height='30' rx='8' fill='white'/>
      <polygon points='180,70 170,90 190,90' fill='white'/>
      "@
    }

    'Confections' {
      @"
      <!-- Donut (even-odd hole) -->
      <circle cx='180' cy='180' r='70' fill='white'/>
      <circle cx='180' cy='180' r='30' fill='$bg'/>
      <!-- Sprinkles -->
      <rect x='160' y='125' width='8' height='16' rx='4' fill='$bg' transform='rotate(20 164 133)'/>
      <rect x='210' y='160' width='8' height='16' rx='4' fill='$bg' transform='rotate(-30 214 168)'/>
      <rect x='132' y='188' width='8' height='16' rx='4' fill='$bg' transform='rotate(35 136 196)'/>
      <rect x='200' y='210' width='8' height='16' rx='4' fill='$bg' transform='rotate(12 204 218)'/>
      "@
    }

    'Dairy Products' {
      @"
      <!-- Cheese wedge -->
      <polygon points='130,230 250,230 220,140 120,160' fill='white'/>
      <circle cx='195' cy='200' r='8' fill='$bg'/>
      <circle cx='215' cy='185' r='6' fill='$bg'/>
      <circle cx='170' cy='210' r='5' fill='$bg'/>
      "@
    }

    'Grains/Cereals' {
      @"
      <!-- Wheat stalk -->
      <rect x='175' y='120' width='10' height='120' rx='5' fill='white'/>
      <!-- grains left -->
      <ellipse cx='160' cy='150' rx='18' ry='10' fill='white' transform='rotate(-20 160 150)'/>
      <ellipse cx='160' cy='180' rx='18' ry='10' fill='white' transform='rotate(-20 160 180)'/>
      <ellipse cx='160' cy='210' rx='18' ry='10' fill='white' transform='rotate(-20 160 210)'/>
      <!-- grains right -->
      <ellipse cx='200' cy='150' rx='18' ry='10' fill='white' transform='rotate(20 200 150)'/>
      <ellipse cx='200' cy='180' rx='18' ry='10' fill='white' transform='rotate(20 200 180)'/>
      <ellipse cx='200' cy='210' rx='18' ry='10' fill='white' transform='rotate(20 200 210)'/>
      "@
    }

    'Meat/Poultry' {
      @"
      <!-- Drumstick -->
      <ellipse cx='205' cy='180' rx='65' ry='50' fill='white'/>
      <rect x='155' y='170' width='55' height='20' rx='10' fill='white'/>
      <circle cx='150' cy='180' r='12' fill='white'/>
      <circle cx='140' cy='180' r='9'  fill='$bg'/>
      "@
    }

    'Produce' {
      @"
      <!-- Apple -->
      <circle cx='180' cy='190' r='60' fill='white'/>
      <path d='M180 135
               c15 -20 40 -15 45 -10
               c-18 -8 -35 5 -45 20z' fill='white'/>
      <rect x='178' y='120' width='6' height='20' rx='3' fill='white'/>
      <path d='M184 120
               c20 -10 30 5 32 15
               c-15 -6 -22 -6 -32 -15z' fill='white'/>
      "@
    }

    'Seafood' {
      @"
      <!-- Fish -->
      <ellipse cx='200' cy='180' rx='60' ry='35' fill='white'/>
      <polygon points='140,180 110,160 110,200' fill='white'/>
      <circle cx='220' cy='175' r='5' fill='$bg'/>
      "@
    }

    Default {
      @"
      <!-- Fallback initial -->
      <text x='50%' y='54%' text-anchor='middle' font-size='120' font-family='Segoe UI, Roboto, Arial' fill='white'>
        $(($name.Substring(0,1)).ToUpper())
      </text>
      "@
    }
  }

  $safeName = Sanitize-FileName $name
  $descEsc = [Security.SecurityElement]::Escape(($desc ?? '').ToString())

  $svg = @"
<svg xmlns='http://www.w3.org/2000/svg' width='$w' height='$h' viewBox='0 0 360 360' role='img' aria-labelledby='title desc'>
  <title>$([Security.SecurityElement]::Escape($name))</title>
  <desc>$descEsc</desc>
  <!-- Badge circle -->
  <circle cx='$cx' cy='$cy' r='$badgeR' fill='$bg' />
  $glyph
  <!-- Metadata -->
  <metadata>
    Generated $(Get-Date -Format o) • CategoryId=$($Row.CategoryId) • CategoryName=$([Security.SecurityElement]::Escape($name))
  </metadata>
</svg>
"@

  $svg | Set-Content -Path $OutPath -Encoding UTF8
}

# --- Generate all ---
foreach ($row in $csvData) {
  $file = Join-Path $outDir ("{0}.svg" -f (Sanitize-FileName $row.CategoryName))
  New-CategorySvg -Row $row -OutPath $file
}

Write-Host "Done. SVGs written to: $outDir"
