using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddKeycloakWebApiAuthentication(configuration);

services
    .AddAuthorization(o => {
            o.AddPolicy("CanWrite",
                b => { b.RequireRealmRoles("wiz:write", "willis:write"); });
            o.AddPolicy("HasWiz",
                b => { b.RequireRealmRoles("wiz:read"); });
        }
    )
    .AddKeycloakAuthorization(configuration)
    .AddAuthorizationServer(configuration)
    ;

var app = builder.Build();

app
    .UseAuthentication()
    .UseAuthorization();

app.MapGet("/partner/willis", () => "You can write!")
    .RequireAuthorization("CanWrite");

app.MapGet("/partner/wiz", (ClaimsPrincipal user) => {
        var id = user.Identity;
        app.Logger.LogInformation("User: {Name}, authenticated: {IsAuthenticated}", id?.Name, id?.IsAuthenticated);
        app.Logger.LogInformation("Audience: {Aud}", user.FindAll(JwtRegisteredClaimNames.Aud).Select(x => x.Value));
        return "You can read!";
    })
    .RequireAuthorization("HasWiz");

app.MapPost("/login", async ([FromBody] LoginRequest login) => {
    if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
        return Results.BadRequest(new {error = "Username and password are required."});

    var client = new HttpClient {
        BaseAddress = new Uri(Environment.GetEnvironmentVariable("KEYCLOAK_REALM_URL")!)
    };
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    var form = new FormUrlEncodedContent([
        new KeyValuePair<string, string>("client_id",
            Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_ID")!),
        new KeyValuePair<string, string>("client_secret",
            Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_SECRET")!),
        new KeyValuePair<string, string>("username", login.Username),
        new KeyValuePair<string, string>("password", login.Password),
        new KeyValuePair<string, string>("grant_type", "password")
    ]);

    var response = await client.PostAsync("protocol/openid-connect/token", form);
    var content = await response.Content.ReadAsStringAsync();
    if (!response.IsSuccessStatusCode)
        return Results.Content(
            content,
            "application/json",
            Encoding.UTF8,
            (int) response.StatusCode
        );

    // Deserialize token response (e.g. { access_token, refresh_token, expires_in, ... })
    var tokenResponse = JsonSerializer.Deserialize<JsonElement>(content);

    // // parse and validate the jwt token
    // var handler = new JwtSecurityTokenHandler();
    // JwtSecurityToken token = handler.ReadJwtToken(tokenResponse.GetProperty("access_token").GetString());
    // app.Logger.LogInformation("Audience: {0}", token.Audiences);

    return Results.Ok(tokenResponse);
});

app.Run();

public record LoginRequest(string Username, string Password);