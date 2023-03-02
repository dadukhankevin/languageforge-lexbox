﻿using System.Net;
using System.Security.Claims;
using LexCore;
using LexCore.Auth;
using LexCore.ServiceInterfaces;
using LexSyncReverseProxy.Config;
using Microsoft.Extensions.Options;

namespace LexSyncReverseProxy.Auth;

public class RestProxyAuthService: IProxyAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly LexBoxApiConfig _lexBoxApiConfig;

    public RestProxyAuthService(IHttpClientFactory clientFactory, IOptionsSnapshot<LexBoxApiConfig> optionsSnapshot)
    {
        _clientFactory = clientFactory;
        _lexBoxApiConfig = optionsSnapshot.Value;
    }

    public async Task<ClaimsPrincipal?> Login(LoginRequest loginRequest)
    {
        var client = _clientFactory.CreateClient("admin");
        var response = await client.PostAsJsonAsync($"{_lexBoxApiConfig.Url}/api/login", loginRequest);
        if (response.StatusCode is HttpStatusCode.Forbidden or HttpStatusCode.Unauthorized)
            return null;
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<LexAuthUser>();

        return user?.GetPrincipal("LexApi");
    }
}