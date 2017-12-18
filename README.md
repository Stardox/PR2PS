# Brief overview

Aim of this project is to recreate the Platform Racing 2 server engine with all functionality (and maybe more).

Server is written in C# and currently consists of several projects organized in Visual Studio solution.

There is possibility of porting this to .NET Core for multiplatform support.

## PR2PS.Web

This represents a central web server. You can think of it as of a PR2Hub 'equivalent'. Server uses ASP.NET WebApi 2 hosted from the console application for easier portability. The web server communicates with separate game servers using SignalR RPC.

## PR2PS.GameServer

This represents an actual TCP game server (Derron, Carina, etc.). The server communicates with the central web server using SignalR RPC.

## PR2PS.ClientBuilder

This is just a simple WinForms application which is used for 'generating' game client so that you can connect to specified web server. This application basically takes a modified Platform Racing 2 client *swf* file along with standalone Flash Projector executable, places it in specified folder and creates *bat* file which will launch the client with specified parameters.

## PR2PS.DataAccess

This acts as data access layer with simple business layer on top of it consumed by PR2PS.Web. Entity Framework (code first approach) is used as ORM, while SQLite is used as database engine for easier portability.

## PR2PS.Common

This libary contains constants, extensions, helper methods and other common constructs. This library is consumend by multiple PR2PS assemblies.

## PR2PS.LevelImporter

This WinForms application can be used to import PR2 maps either from the local files or from original PR2 server into PR2PS database.

# Features

Here is short list of stuff what is and what is not working at this point.

## Implemented

- Account creation
- Logging in and logging out
- Chatrooms
- Private messaging
- Ban system
- Promotion and demotion system
- Campaign, Search and Newest tabs
- 90% functional multiplayer
- Level editor
- Local level host

## Not implemented

- Experience system
- Prize system
- Friend list and Ignore list functionality
- Private message reporting
- Web server UI
- All time best and Today's best tabs
- User warnings
- Temporary and trial moderators
- Guilds
- Many more

# Set up

## Warning!

This server is not safe to be hosted publicly. Many security checks have not been implemented yet and encryption has been disabled in the client as well. Test it only with people that you trust.

## PR2PS.Web

PR2PS.Web has to launched first in the following fashion: <br />
`PR2PS.Web.exe host_url search_url`

Where:
- *host_url*	- Is address, where the web server should be hosted at.
- *search_url*	- Optional argument. Indicates address of search API. Use `http://pr2hub.com/search_levels.php` if you want the original PR2 server to handle the searching. Otherwise you can leave this blank and PR2PS.Web will handle searching of its local database.

Example: <br />
`PR2PS.Web.exe http://127.0.0.1:12345` <br />

If no arguments will be supplied then the web server will be launched with the following configuration: <br />
`PR2PS.Web.exe http://127.0.0.1:8080` <br />

After the startup, database files *PR2DB.sqlite* and *PR2Levels.sqlite* will be generated and seeded (if they have not been already). You can delete the database files anytime if you want to start over.

## PR2PS.GameServer

Next up it is neccessary to launch one or more instances of PR2PS.GameServer in following fashion: <br />
`PR2PS.GameServer.exe host_ip host_port web_url server_name`

Where:
- *host_ip*		- Is IPv4 address to which the TCP listener will be bound.
- *host_port*	- Is port to which the TCP listener will be bound.
- *web_url*		- Is the url of web server.
- *server_name*	- Is the name of the server (Derron, Carina, Asdf, etc.).

Example: <br />
`PR2PS.GameServer.exe 127.0.0.1 9000 http://127.0.0.1:12345 MyServer` <br />

If no arguments will be supplied then the game server will be launched with the following configuration: <br />
`PR2PS.GameServer.exe 127.0.0.1 9160 http://127.0.0.1:8080 Local`

After the startup, the game server will attempt to contact web server so that it can be added to the list of playable game servers.

## PR2PS.ClientBuilder

Final step includes making the game client to connect to the specified address. This tool has been created for this purpose.

After launching the tool, following parameters has to be supplied:

**Standalone Flash Projector executable** <br />
This file is not included in this project and has to be downloaded manually from Adobe site: <br />
https://www.adobe.com/support/flashplayer/debug_downloads.html <br />
On the download page look for *Download the Flash Player projector*, file should be called: <br />
`flashplayer_XY_sa.exe`

**Modified Platform Racing 2 client** <br />
The *swf* file is included in same folder as ClientBuilder and is called *client.swf*.

**Web server url** <br />
Put the web server (PR2PS.Web) host address here.

**Levels server url** <br />
If you provided `search_url` of the original PR2 server when launching the PR2PS.Web then you need to specify `http://pr2hub.com/levels` here so that maps are correctly downloaded from original PR2 server. <br />
If you did not provide `search_url` when launching the PR2PS.Web then you need to specify web server url here followed by `/levels`.

**Destination folder** <br />
This is where all the client files will be copied to. After the build, launch the client using *Run.bat*.

# Original work

This project is based on game of Jiggmin (Jacob Grahn) called Platform Racing 2.

## Jiggmin
https://github.com/Jiggmin <br />
https://www.youtube.com/user/Jiggmin

## Platform Racing 2 game
http://pr2hub.com/
