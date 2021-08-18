@echo off
xcopy /s "C:\Users\damie\source\repos\SubnauticaMod\ClientSubnautica\bin\Release\ClientSubnautica.dll" "E:\Jeux\Subnautica Below Zero\QMods\SubnauticaMultiplayerMod" /Y
start "" "C:\Users\damie\source\repos\SubnauticaMod\ServerSubnautica\bin\Release\net5.0\ServerSubnautica.exe"
start "" "E:\Jeux\Subnautica Below Zero\SubnauticaZero.exe"