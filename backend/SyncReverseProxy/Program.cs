using LexCore.ServiceInterfaces;
using LexSyncReverseProxy;
using LexSyncReverseProxy.Auth;
using LexSyncReverseProxy.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions<LexBoxApiConfig>()
    .BindConfiguration("LexBoxApi")
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddScoped<IProxyAuthService, RestProxyAuthService>();
builder.Services.AddHttpClient();
builder.Services.AddSyncProxy(builder.Configuration, builder.Environment);
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
                            HttpLoggingFields.ResponsePropertiesAndHeaders | 
                            HttpLoggingFields.RequestBody |
                            HttpLoggingFields.ResponseBody;
    options.MediaTypeOptions.AddText("text/html");
    options.RequestHeaders.Add(HeaderNames.Authorization);
    options.ResponseHeaders.Add(HeaderNames.Authorization);
    options.RequestHeaders.Add(HeaderNames.WWWAuthenticate);
    options.ResponseHeaders.Add(HeaderNames.WWWAuthenticate);
});


var app = builder.Build();

app.UseHttpLogging();

// app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapSyncProxy();
app.Run();