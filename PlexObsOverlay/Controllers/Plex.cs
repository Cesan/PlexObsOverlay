using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using PlexObsOverlay.Hubs;
using PlexObsOverlay.ViewModels;

namespace PlexObsOverlay.Controllers;

public class Plex : Controller
{
    private readonly ILogger<Plex> logger;
    private readonly IHubContext<MetadataHub> metadataHub;
    private readonly IConfiguration configuration;

    private static readonly OverlayViewModel OverlayViewModel = new();

    private static FileStream? metadataFile;

    public Plex(ILogger<Plex> logger, IHubContext<MetadataHub> metadataHub, IConfiguration configuration)
    {
        this.logger = logger;
        this.metadataHub = metadataHub;
        this.configuration = configuration;

        metadataFile ??=
            System.IO.File.Open(Path.Combine("wwwroot", "current.txt"), FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.Read);
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Overlay()
    {
        HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");

        return View(OverlayViewModel);
    }

    [HttpPost]
    public async Task WebhookPost()
    {
        var form = HttpContext.Request.Form;

        var payload = form["payload"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(payload)) return;

        var json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(payload)!;

        var action = (json.@event as JValue)?.Value as string;

        var accountInfo = json.Account;

        var userName = (accountInfo.title as JValue)?.Value as string;

        logger.LogInformation("Received webhook from {UserName} for {Action}", userName, action);

        var configUsername = configuration["Plex:Username"];

        var allUsers = string.IsNullOrWhiteSpace(configUsername) || configUsername.Equals("*");

        if (action != "media.play" || (!userName!.Equals(configUsername) && !allUsers)) return;

        await HandleThumbnail();

        await HandleMetadata(json.Metadata);

        await metadataHub.Clients.All.SendAsync("update");
    }

    private async Task HandleMetadata(dynamic metadata)
    {
        var title = (metadata.title as JValue)?.Value as string;
        var artist = (metadata.grandparentTitle as JValue)?.Value as string;

        OverlayViewModel.Title = title ?? "Unknown";
        OverlayViewModel.Artist = artist ?? "Unknown";

        if (metadataFile == null) return;

        metadataFile.Seek(0, SeekOrigin.Begin);
        metadataFile.SetLength(0);

        await metadataFile.WriteAsync(Encoding.Default.GetBytes($"{artist} - {title}"));
        await metadataFile.FlushAsync();

        logger.LogInformation("Current Song: {Title} by {GrandparentTitle}", title, artist);
    }

    private async Task HandleThumbnail()
    {
        var files = HttpContext.Request.Form.Files;

        if (files.Count == 0) return;

        var image = files.FirstOrDefault(x => x.ContentType.Contains("image"));

        if (image == null) return;

        await using var fileStream = System.IO.File.OpenWrite(Path.Combine("wwwroot", image.FileName));

        await image.CopyToAsync(fileStream);
    }
}