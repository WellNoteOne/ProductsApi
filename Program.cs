using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);
var googleClientId = "490439995551-n1pm61l6p43f4uqq31rro48oimt7sj7h.apps.googleusercontent.com";

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ProductsDb"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://accounts.google.com";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Varmistaa, että token on Googlen myöntämä
            ValidIssuer = "https://accounts.google.com",
            ValidateAudience = true, // Varmistaa, että token on tarkoitettu tälle API:lle
            ValidAudience = googleClientId, // Aseta Google Client ID tähän
            ValidateLifetime = true // Varmistaa, että token ei ole vanhentunut
        };
    });


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseDefaultFiles();
app.UseStaticFiles();

//app.UseHttpsRedirection();
app.UseAuthentication(); // Käynnistää autentikointijärjestelmän
app.UseAuthorization(); // Tarkistaa, onko käyttäjällä oikeudet

app.MapControllers();

app.Run();
