/*==============================================================================================================================
| Author        Ignia, LLC
| Client        GoldSim
| Project       Media Website
\=============================================================================================================================*/
using Microsoft.AspNetCore.StaticFiles;
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
| Configure: Static file handling with cache headers
\-----------------------------------------------------------------------------------------------------------------------------*/
var provider                    = new FileExtensionContentTypeProvider();
const int duration              = 60 * 60 * 24 * 365 * 2;                    // 63072000 seconds; i.e., two years

provider.Mappings[".webmanifest"] = "application/manifest+json";

provider.Mappings[".emf"]   = "image/x-emf";
provider.Mappings[".m4s"]   = "video/mp4";
provider.Mappings[".mpd"]   = "application/dash+xml";
provider.Mappings[".m3u8"]  = "application/x-mpegURL";

var staticFileOptions           = new StaticFileOptions {
  ContentTypeProvider           = provider,
  OnPrepareResponse             = context => {
    context.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + duration;
  }
};

app.UseStaticFiles(staticFileOptions);

/*------------------------------------------------------------------------------------------------------------------------------
| Configure: Default services
\-----------------------------------------------------------------------------------------------------------------------------*/
app.MapGet("/", () => "Hello World!");

/*------------------------------------------------------------------------------------------------------------------------------
| Run application
\-----------------------------------------------------------------------------------------------------------------------------*/
app.Run();
