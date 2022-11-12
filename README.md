# Subnautica Below Zero Multiplayer Mod
A multiplayer mod for Subnautica Below Zero !

## Important
Please do not forget to modify these before using:  
- **[DEVELOPERS]** "Directory.Build.props" at the root of the mod, replace the path by your subnautica path.
- ip address to host the server in config.json file (if the one created is not the one you want to use)  

Please install [**.NET Framework 4.7.2**](https://dotnet.microsoft.com/download/dotnet-framework/net472 ".NET Framework 4.7.2") and set it for the ClientSubnautica project.  
Please install [**.NET 5.0**](https://dotnet.microsoft.com/download/dotnet/5.0 ".NET 5.0") and set it for the ServerSubnautica project.  

## Using
Open solution in Visual Studio and build the whole project. It will build project, setup the mod DLL, start the server and subnautica  

### Configuration
If you launched the game without making or moving a save file for the server from the game saves, please do it:
1. Go to `/SubnauticaZero/SNAppData/SavedGames/` and copy any of the `slotXXXX` folder.
2. Paste it into `/SubnauticaServer/`. It should look like this: `/SubnauticaServer/slot0000/` for `slot0000`.
3. Into `/SubnauticaServer/`, open `config.json`, and change the `MapFolderName` value to the name of the folder. For example: `slot0000`:
```json
	"MapFolderName": "slot0000"
```
4. Start `ServerSubnautica.exe`

## Subnautica Below Zero Multiplayer
[**SBZ Multiplayer Discord server**](https://discord.gg/Nr6nBdCUg2 "SBZM Discord")

