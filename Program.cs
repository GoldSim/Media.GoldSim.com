/*==============================================================================================================================
| Author        Ignia, LLC
| Client        GoldSim
| Project       Media Website
\=============================================================================================================================*/
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

/*==============================================================================================================================
| ENABLE SERVICES
\-----------------------------------------------------------------------------------------------------------------------------*/
var builder = WebApplication.CreateBuilder(args);

/*==============================================================================================================================
| CONFIGURE: APPLICATION
\-----------------------------------------------------------------------------------------------------------------------------*/
var app = builder.Build();

/*------------------------------------------------------------------------------------------------------------------------------
| Configure: Environment-specific features
\-----------------------------------------------------------------------------------------------------------------------------*/
if (app.Environment.IsProduction()) {
  app.UseHttpsRedirection();
  app.UseHsts();
}

/*------------------------------------------------------------------------------------------------------------------------------
| Configure: Default files (e.g., Index.html)
\-----------------------------------------------------------------------------------------------------------------------------*/
app.UseDefaultFiles();

/*------------------------------------------------------------------------------------------------------------------------------
| Configure: Content Type Provider
\-----------------------------------------------------------------------------------------------------------------------------*/
var provider                    = new FileExtensionContentTypeProvider();
const int duration              = 60 * 60 * 24 * 365 * 2;       // 63072000 seconds; i.e., two years

/*------------------------------------------------------------------------------------------------------------------------------
| Configure: Custom MIME Types
\-----------------------------------------------------------------------------------------------------------------------------*/
provider.Mappings[".webmanifest"] = "application/manifest+json";
provider.Mappings[".emf"]   = "image/x-emf";
provider.Mappings[".m4s"]   = "video/mp4";
provider.Mappings[".mpd"]   = "application/dash+xml";
provider.Mappings[".m3u8"]  = "application/x-mpegURL";
provider.Mappings[".gsm"]   = "application/octet-stream";
provider.Mappings[".gsp"]   = "application/octet-stream";

/*------------------------------------------------------------------------------------------------------------------------------
| Configure: Alternate Directories
\-----------------------------------------------------------------------------------------------------------------------------*/
registerDirectory("wwwroot", "");
registerDirectory("Documents");
registerDirectory("Images");
registerDirectory("Videos");

/*------------------------------------------------------------------------------------------------------------------------------
| Run application
\-----------------------------------------------------------------------------------------------------------------------------*/
app.Run();

/*==============================================================================================================================
| METHOD: REGISTER DIRECTORY
\-----------------------------------------------------------------------------------------------------------------------------*/
void registerDirectory(string path, string? requestPath = null) =>
  app!.UseStaticFiles(new StaticFileOptions() {
    ContentTypeProvider = provider,
    OnPrepareResponse = context => {
      context.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + duration;
    },
    FileProvider = new PhysicalFileProvider(Path.Combine(builder!.Environment.ContentRootPath, path)),
    RequestPath = requestPath ?? "/" + path
  });