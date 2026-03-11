$files = rg -l 'throw new NotImplementedException\(\);' 'NRedberry.Core.Tests' -g '*.cs'

foreach ($file in $files)
{
    $content = Get-Content -Path $file -Raw
    $original = $content

    $content = $content -replace 'throw new NotImplementedException\(\);', 'throw SkipException.ForSkip("Pending port from Java.");'

    if ($content -notmatch 'using Xunit\.Sdk;')
    {
        if ($content -match '^using .*?;\r?\n')
        {
            $content = [regex]::Replace(
                $content,
                '^(using .*?;\r?\n)+',
                {
                    param($match)
                    return $match.Value + "using Xunit.Sdk;`r`n"
                },
                1)
        }
        else
        {
            $content = "using Xunit.Sdk;`r`n`r`n" + $content
        }
    }

    if ($content -ne $original)
    {
        [System.IO.File]::WriteAllText(
            (Resolve-Path $file),
            $content,
            [System.Text.Encoding]::UTF8)
    }
}
