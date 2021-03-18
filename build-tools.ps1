# Cleanup
Remove-Item -Recurse -Force ./tools

# Build the tools
Write-Host "Compiling font generator for linux"
dotnet publish -c Release -r linux-x64 -o ./tools/linux-x64/fontgenerator --nologo --self-contained --force -v q ./src/Arqanore.FontGenerator/Arqanore.FontGenerator.csproj

Write-Host "Compiling font generator for windows"
dotnet publish -c Release -r win-x64 -o ./tools/win-x64/fontgenerator --nologo --self-contained --force -v q ./src/Arqanore.FontGenerator/Arqanore.FontGenerator.csproj

Write-Host "Compiling tex generator for linux"
dotnet publish -c Release -r linux-x64 -o ./tools/linux-x64/texgenerator --nologo --self-contained --force -v q ./src/Arqanore.TexGenerator/Arqanore.TexGenerator.csproj

Write-Host "Compiling tex generator for windows"
dotnet publish -c Release -r win-x64 -o ./tools/win-x64/texgenerator --nologo --self-contained --force -v q ./src/Arqanore.TexGenerator/Arqanore.TexGenerator.csproj
