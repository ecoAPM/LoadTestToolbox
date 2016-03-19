del /s /q bin
dotnet restore common drill hammer
dotnet build common -o bin -f net461 -c Release
dotnet build drill  -o bin -f net461 -c Release
dotnet build hammer -o bin -f net461 -c Release