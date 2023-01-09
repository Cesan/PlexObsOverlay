![firefox_hvU2cKXz7m](https://user-images.githubusercontent.com/33752516/211348722-bfd1d9bb-9b6b-4a4b-8be2-065c7739c60f.png)

# About PlexObsOverlay

PlexObsOverlay is a simple and lightweight ASP.NET Core application which uses Plex webhooks to generate a browser rendered OBS overlay for the currently playing song which can be used directly as a browser source.

# Using PlexObsOverlay

- ⚠️ As the PlexObsOverlay relies on Plex Webhooks which is currently a Plex Pass only feature, you can only use it on Plex accounts which have a active Plex Pass subscription! ⚠️

### Setup

To use this first download the latest release on the [releases page](https://github.com/Cesan/PlexObsOverlay/releases) and put it where you want to run it. 
> Recommended to run either on the same machine as the Plex server or in the same local network as both the Plex server and OBS

Also download and install the latest [.NET 7 runtime](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) if not installed already.

Next set up the webhook on your plex account pointing to the address you are using this program on (e.g. `http://192.168.0.2:xxxx` where 'xxxx' is the port which defaults to 5727). 
> If you don't know how to set up a webhook on your plex account see this [article by plex](https://support.plex.tv/articles/115002267687-webhooks/)

### Config

PlexObsOverlay should run out of the box and work in every scenario, to configure the behavior more specifically the `appsettings.json` file provides configurable properties like the address (defaults to `http://*:5727` which listens on all network interfaces) and the Plex username the overlay should listen for (defaults to `*` which will listen on all users of your Plex account)
> Further instructions on how to set up properties is described in the file as comments

### Running

Now that the webhook is set up you can start the PlexObsOverlay application. Metadata is exchanged only on song changes, so the first visible update happens after the song changes for the first time.

You can now use the overlay as a browser source on OBS using the ip address the PlexObsOverlay is running on.
> The default height of the overlay is 120px, this can be changed in the `wwwroot/css/site.css` file

# Preview

![obs64_IyHwSiMboU](https://user-images.githubusercontent.com/33752516/211349445-37593043-b047-46f1-9084-028f97b82ffe.png)
