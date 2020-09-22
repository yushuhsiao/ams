rd /q /s .\pub
dotnet clean -c Debug
dotnet clean -c Release
dotnet publish -c Release -o .\pub -r linux-x64 --self-contained true
@rem dotnet publish -c Release -o .\pub --self-contained true -r win-x64
