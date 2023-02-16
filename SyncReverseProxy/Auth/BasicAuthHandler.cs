﻿using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using LexCore.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace LexSyncReverseProxy.Auth;
public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthScheme = "HgAuthScheme";
    private readonly IProxyAuthService _proxyAuthService;

    public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IProxyAuthService proxyAuthService) : base(options, logger, encoder, clock)
    {
        _proxyAuthService = proxyAuthService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //the Basic realm part is required by the HG client, otherwise it won't request again with a basic auth header
        Response.Headers.WWWAuthenticate = "Basic,Basic realm=\"SyncProxy\"";
        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader))
        {
            return AuthenticateResult.Fail("No authorization header");
        }

        var basicAuthValue = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader["Basic ".Length..])).Split(":");
        var (username, password) = basicAuthValue switch
        {
            ["", ""] => (null, null),
            [var u, var p] => (u, p),
            _ => (null, null)
        };
        if (username is null || password is null)
        {
            return AuthenticateResult.Fail("Invalid Request");
        }

        if (!await _proxyAuthService.IsAuthorized(username, password))
            return AuthenticateResult.Fail("Invalid username or password");
        
        var claimsIdentity = new ClaimsIdentity(new []
        {
            new Claim(ClaimTypes.Name, username)
        }, "Basic");
        return AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name)
        );
    }
}