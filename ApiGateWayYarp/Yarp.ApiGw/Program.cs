using Microsoft.AspNetCore.Authentication.BearerToken;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services
    .AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken(); // obtain a earer token from the incoming request and set the user principal

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("first-api-access", policy =>
        {
            policy.RequireAuthenticatedUser().RequireClaim("first-api-access", true.ToString());
        });

        options.AddPolicy("second-api-access", policy =>
        {
            policy.RequireAuthenticatedUser().RequireClaim("second-api-access", true.ToString());
        });
    });

var app = builder.Build();

app.MapGet("login", (bool firstApi = false, bool secondApi = false) => Results.SignIn(new ClaimsPrincipal(new ClaimsIdentity([
    new Claim("sub", Guid.NewGuid().ToString()),
    new Claim("first-api-access", firstApi.ToString()),
    new Claim("second-api-access", secondApi.ToString()),
    ], BearerTokenDefaults.AuthenticationScheme)), authenticationScheme: BearerTokenDefaults.AuthenticationScheme));


app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();