@echo off
xcopy /s "C:\Users\damie\source\repos\SubnauticaModTest\SubnauticaModTest\bin\Release\SubnauticaModTest.dll" "E:\Jeux\Subnautica Below Zero\QMods\SubnauticaModTest" /Y
start "" "C:\Users\damie\source\repos\ServerSubnautica\ServerSubnautica\bin\Release\net5.0\ServerSubnautica.exe"
start "" "E:\Jeux\Subnautica Below Zero\SubnauticaZero.exe"