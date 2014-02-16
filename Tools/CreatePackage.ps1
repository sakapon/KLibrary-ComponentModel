$msbuild = "C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"

cd ..\KLibrary2_1
Invoke-Expression ($msbuild + " KLibrary2_1.sln /p:Configuration=Release /t:Clean")
Invoke-Expression ($msbuild + " KLibrary2_1.sln /p:Configuration=Release /t:Rebuild")

cd .\ComponentModel
.\NuGetPackup.exe

move *.nupkg ..\..\Published -Force
