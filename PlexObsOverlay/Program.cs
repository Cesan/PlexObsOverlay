using PlexObsOverlay.Controllers;
using PlexObsOverlay.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddMvc(options => { options.EnableEndpointRouting = false; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (!ctx.File.Name.Contains("jpg")) return;

        ctx.Context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
        ctx.Context.Response.Headers.ETag = DateTime.Now.Ticks.ToString();
    }
});

app.UseMvc();

app.UseAuthorization();

//app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Plex}/{action=Index}/{id?}");

app.MapHub<MetadataHub>("/metadataHub");

app.Run();