/*==============================================================================================================================
| Author        Ignia, LLC
| Client        GoldSim
| Project       Media Website
\=============================================================================================================================*/
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
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
| Configure: Static File Options
\-----------------------------------------------------------------------------------------------------------------------------*/
var staticFileOptions           = new StaticFileOptions {
  ContentTypeProvider           = provider,
  OnPrepareResponse             = context => {
    context.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + duration;
  }
};

/*------------------------------------------------------------------------------------------------------------------------------
| Configure: Alternate Directories
\-----------------------------------------------------------------------------------------------------------------------------*/
registerDirectory(staticFileOptions, "wwwroot", "");
registerDirectory(staticFileOptions, "Documents");
registerDirectory(staticFileOptions, "Images");
registerDirectory(staticFileOptions, "Videos");

/*------------------------------------------------------------------------------------------------------------------------------
| Run application
\-----------------------------------------------------------------------------------------------------------------------------*/
app.Run();

/*==============================================================================================================================
| METHOD: REGISTER DIRECTORY
\-----------------------------------------------------------------------------------------------------------------------------*/
void registerDirectory(StaticFileOptions options, string path, string? requestPath = null) =>
  app!.UseStaticFiles(new StaticFileOptions {
    FileProvider                = new PhysicalFileProvider(Path.Combine(builder!.Environment.ContentRootPath, path)),
    RequestPath                 = requestPath?? "/" + path
  });