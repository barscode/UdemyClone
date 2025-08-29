using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UdemyClone.Api.Data;
using UdemyClone.Api.Domain;
using UdemyClone.Api.DTOs;
using UdemyClone.Api.Application.UseCases;
using UdemyClone.Api.Infrastructure.Repositories;
using UdemyClone.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});
 builder.Services.ConfigureHttpJsonOptions(options =>  {   options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;  });
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();
// Repositories and services
builder.Services.AddScoped(typeof(UdemyClone.Api.Infrastructure.Repositories.IGenericRepository<>), typeof(UdemyClone.Api.Infrastructure.Repositories.EfGenericRepository<>));
builder.Services.AddScoped<UdemyClone.Api.Application.UseCases.CreateCouponUseCase>();
// Application services
builder.Services.AddScoped<UdemyClone.Api.Services.IMessageService, UdemyClone.Api.Services.MessageService>();
builder.Services.AddScoped<UdemyClone.Api.Services.ICommentService, UdemyClone.Api.Services.CommentService>();
builder.Services.AddScoped<UdemyClone.Api.Services.IEnrollmentService, UdemyClone.Api.Services.EnrollmentService>();
builder.Services.AddScoped<UdemyClone.Api.Services.ILessonCompletionService, UdemyClone.Api.Services.LessonCompletionService>();

var app = builder.Build();

// DB init (EnsureCreated for quick start)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.EnsureDeleted(); // For development: delete existing database to apply new schema
    db.Database.EnsureCreated();
    if (!db.Set<UdemyClone.Api.Domain.Kategori>().Any())
    {
        db.Set<UdemyClone.Api.Domain.Kategori>().AddRange(
            new UdemyClone.Api.Domain.Kategori { Ad = "Development" },
            new UdemyClone.Api.Domain.Kategori { Ad = "Business" },
            new UdemyClone.Api.Domain.Kategori { Ad = "Design" }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
