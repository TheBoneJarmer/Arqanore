$files = [System.IO.Directory]::GetFiles("./shaders");
$writer = New-Object System.IO.StreamWriter 'include/arqanore/shadersources.h'

$writer.WriteLine("#pragma once");
$writer.WriteLine("");

foreach ($file in $files)
{
    $filename = [System.IO.Path]::GetFileNameWithoutExtension($file);
    $lines = [System.IO.File]::ReadAllLines($file);
    $shaderName = "SHADER_" + $filename.ToUpper();
    $shaderBody = "";

    foreach ($line in $lines)
    {
        $shaderBody += "$line\n"
    }

    $writer.WriteLine("#define $shaderName `"" + $shaderBody + "`"");
}

$writer.Close();