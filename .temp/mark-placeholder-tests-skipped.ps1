$files = rg -l 'SkipException\.ForSkip\("Pending port from Java\."\);' 'NRedberry.Core.Tests' -g '*.cs'

foreach ($file in $files)
{
    $content = Get-Content -Path $file -Raw
    $original = $content

    $content = $content -replace '\[Fact\]', '[Fact(Skip = "Pending port from Java.")]'
    $content = $content -replace '\[Theory\]', '[Theory(Skip = "Pending port from Java.")]'

    if ($content -ne $original)
    {
        [System.IO.File]::WriteAllText(
            (Resolve-Path $file),
            $content,
            [System.Text.Encoding]::UTF8)
    }
}
