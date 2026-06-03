using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using API.Services.Options;
using API.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var httpsCertPath = builder.Configuration["Https:CertPath"];
var httpsCertPassword = builder.Configuration["Https:CertPassword"];

if (!string.IsNullOrEmpty(httpsCertPath))
{
    var resolvedCertPath = Path.IsPathRooted(httpsCertPath)
        ? httpsCertPath
        : Path.Combine(builder.Environment.ContentRootPath, httpsCertPath);

    if (File.Exists(resolvedCertPath))
    {
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenLocalhost(5093); 
        });
    }
}

var jwtKey = builder.Configuration["Jwt:Key"];

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)
        )
    };
});

builder.Services.AddAuthentication();

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));
    
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<AccountService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://secureloginapp.ddns.net:5173", "https://secureloginapp.ddns.net")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();