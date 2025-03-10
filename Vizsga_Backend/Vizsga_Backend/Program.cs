using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Vizsga_Backend.Models;
using Vizsga_Backend.Services;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDB és egyéb szolgáltatások regisztrálása
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<UsersFriendlyStatService>();
builder.Services.AddSingleton<UsersTournamentStatService>();
builder.Services.AddSingleton<JwtService>();
builder.Services.AddSingleton<MessageService>();
builder.Services.AddSingleton<AnnouncedTournamentService>();
builder.Services.AddSingleton<MatchHeaderService>();

// Cloudinary regisztrálása
builder.Services.AddSingleton(serviceProvider =>
{
    var cloudinarySettings = serviceProvider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    var account = new Account(
        cloudinarySettings.CloudName,
        cloudinarySettings.ApiKey,
        cloudinarySettings.ApiSecret
    );
    return new Cloudinary(account);
});

// Authentication konfigurálása
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

// Swagger konfigurálása
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "https://disciplinary-marj-feketemiklos222-91053eff.koyeb.app", "https://darts-vizsgaremek.vercel.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// A környezeti változóból történõ portok beállítása
var httpPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORT") ?? "8080";
var httpsPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT") ?? "8081";

app.UseSwagger();
app.UseSwaggerUI();

// HTTPS átirányítás
app.UseHttpsRedirection();

// CORS beállítása
app.UseCors("AllowSpecificOrigins");

// Autentikáció és autorizáció middleware-ek
app.UseAuthentication();
app.UseAuthorization();

// A vezérlõk térképezése
app.MapControllers();

// A dinamikus port beállítása
app.Run(); // HTTP port használata
