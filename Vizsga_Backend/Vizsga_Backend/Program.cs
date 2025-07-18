using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Vizsga_Backend.Interfaces;
using Vizsga_Backend.Models;
using Vizsga_Backend.Services;
using Vizsga_Backend.SignalR;
using VizsgaBackend.Models;
using VizsgaBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDB �s egy�b szolg�ltat�sok regisztr�l�sa
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IUsersFriendlyStatService, UsersFriendlyStatService>();
builder.Services.AddSingleton<IUsersTournamentStatService, UsersTournamentStatService>();
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IMessageService, MessageService>();
builder.Services.AddSingleton<IAnnouncedTournamentService, AnnouncedTournamentService>();
builder.Services.AddSingleton<IMatchHeaderService, MatchHeaderService>();
builder.Services.AddSingleton<IMatchService, MatchService>();

// Cloudinary regisztr�l�sa
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

// Authentication konfigur�l�sa
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

// SignalR konfigur�l�sa Azure SignalR-rel
builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(30);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(120);
})
    .AddAzureSignalR(builder.Configuration["SignalR:ConnectionString"]);

// Swagger konfigur�l�sa
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
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// A k�rnyezeti v�ltoz�b�l t�rt�n� portok be�ll�t�sa
var httpPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTP_PORT") ?? "8080";
var httpsPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT") ?? "8081";

app.UseSwagger();
app.UseSwaggerUI();

// HTTPS �tir�ny�t�s
app.UseHttpsRedirection();

// CORS be�ll�t�sa
app.UseCors("AllowSpecificOrigins");

// Autentik�ci� �s autoriz�ci� middleware-ek
app.UseAuthentication();
app.UseAuthorization();

// SignalR Hub hozz�ad�sa
app.MapHub<MatchHub>("/matchHub");

// A vez�rl�k t�rk�pez�se
app.MapControllers();

// A dinamikus port be�ll�t�sa
app.Run(); // HTTP port haszn�lata
