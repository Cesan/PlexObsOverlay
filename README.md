# About PlexObsOverlay

PlexObsOverlay is a simple and lightweight ASP.NET Core application which uses Plex webhooks to generate a browser rendered OBS overlay for the currently playing song which can be used directly as a browser source.

# Using PlexObsOverlay

- ⚠️ As the PlexObsOverlay relies on Plex Webhooks which is currently a Plex Pass only feature, you can only use it on Plex accounts which have a active Plex Pass subscription! ⚠️

To use this first download the latest release on the [releases page](https://github.com/Cesan/PlexObsOverlay/releases) and put it where you want to run it.
> Recommended to run either on the same machine as the Plex server or in the same local network as both the Plex server and OBS

Next set up the webhook on your plex account pointing to the address you are using this program on (e.g. `http://192.168.0.2:xxxx` where 'xxxx' is the port which defaults to 5727). 
> If you don't know how to set up a webhook on your plex account see this [article by plex](https://support.plex.tv/articles/115002267687-webhooks/)

Now that the webhook is set up you can start the PlexObsOverlay application. Metadata is exchanged only on song changes, so the first visible update happens after the song changes for the first time.

You can now use the overlay as a browser source on OBS using the ip address the PlexObsOverlay is running on.
> The default height of the overlay is 120px, this can be changed in the `wwwroot/css/site.css` file
