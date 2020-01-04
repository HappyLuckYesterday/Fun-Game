# Rhisis

[![forthebadge](http://forthebadge.com/images/badges/made-with-c-sharp.svg)](http://forthebadge.com)
[![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com)

[![Build Status](https://travis-ci.org/Eastrall/Rhisis.svg?branch=develop)](https://travis-ci.org/Eastrall/Rhisis)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/500148ec8bdd4f2e954f11c682c39f3c)](https://www.codacy.com/app/Eastrall/Rhisis?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Eastrall/Rhisis&amp;utm_campaign=Badge_Grade)
[![discord](https://discordapp.com/api/guilds/294405146300121088/widget.png)](https://discord.gg/zAT6Az2)

Rhisis is a FlyForFun V15 emulator built with C# 7 and the .NET Core Framework 3.0.

This project has been created for learning purposes about the network and game logic problematics on the server-side.
We choose to use the [Sylver.Network][sylvernetwork] to manage our server connecitions because it provides a clients management system and also a robust packet management system entirely customisable.

> :warning: This project is not affiliated with **Gala Lab**.

## Technical environment and details

- Language: `C#` 8 (latest)
- Framework: `.NET Core 3.0`
- Application type: `Console`
- Database type: `MySQL`
- Configuration files type: `JSON`
- External libraries used:
	- [Sylver.Network][sylvernetwork]
	- [Sylver.HandlerInvoker](https://github.com/Eastrall/Sylver.HandlerInvoker)
	- [Entity Framework Core](https://github.com/aspnet/EntityFrameworkCore)
	- [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
	- [NLog](https://github.com/NLog/NLog)
- Environment: Visual Studio 2019

## Features

### Common
- Logger
- Rijndael cryptography algorithm
- Custom exceptions
- Packet handler

### Database
- MySQL database support

### Login
- Inter-Server authentication process (CoreServer)
- Client authentication process
- Send server list to connected client

### Cluster
- Inter-Server authentication (CoreClient)
- Character list
- Create character
- Delete character
- 2nd password verification
- Pre join

### World
- Inter-Server authentication (CoreClient)
- Entity Component System architecture
- Connect to the world
- Load resources
   - Defines & texts
   - Monsters
   - Maps
   - Items
   - NPC Data/Shops/Dialogs
   - Job Data
   - Exp table
   - Behaviors (AI)
- Spawn monsters and NPC
- Visibility System
- Mobility System
- Respawn System
- Chat System
	- Chat commands:
		- Create item : `/ci` or `/createitem`
		- Get gold : `/getgold`
		- Teleport : `/teleport`
- Inventory System
	- Move items
	- Equip/Unequip items
	- Save inventory
	- Drop items on the ground
	- Item usage (food, potion, refreshers)
- Shop System
	- Buy items
	- Sell items
- Trade System
- NPC Dialog System
- Drop System
	- Pickup Gold / Items
- Battle System
	- Melee Attack
		- Player VS Monster
	- Monster death
	- Monster item/gold drop
- Character customization system
- Attribute System
- Quest System


## How to setup Rhisis (from `develop` branch) (Windows platform)

1. Download or Clone the `develop` branch.
2. Install the latest [.NET Core 3.X SDK](https://dotnet.microsoft.com/download)
3. Install [`MySQL Server`](https://dev.mysql.com/downloads/installer/)
4. Go to your Rhisis folder, open a `cmd` or `PowerShell` and compile the solution with the command : `dotnet build`
5. Go to the `bin/` folder, open a `cmd` or `PowerShell` and type the following commands: 
```
$ ./rhisis-cli.bat database initialize
$ ./rhisis-cli.bat configure login
$ ./rhisis-cli.bat configure cluster
$ ./rhisis-cli.bat configure world
```
> ℹ️ The `rhisis-cli database initialize` command will guide you through the rhisis database configuration and will setup the database for you.
6. Create an account using the `./rhisis-cli user create` command
8. Start the emulator
- Start `1.login.bat`
- Start `2.cluster.bat`
- Start `3.world.bat`

## Contributors

- [Eastrall](https://github.com/Eastrall)
- [Steve-Nzr](https://github.com/Steve-Nzr)
- [Freezeraid](https://github.com/Freezeraid)
- [Skeatwin](https://github.com/Skeatwin)
- [johmarjac](https://github.com/johmarjac)
- [Kaev](https://github.com/Kaev)
- [YarinNet](https://github.com/YarinNet)
- [Almewty](https://github.com/Almewty)
- [Anjuts](https://github.com/Anjuts)
- [MarkWilds](https://github.com/MarkWilds)

## Supporters

- Ukiyo
- Kinami
- Sauce

[sylvernetwork]: https://github.com/Eastrall/Sylver.Network
