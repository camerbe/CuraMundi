using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Domain.Entities;
using CuraMundi.Infrastructure.DAL.Data;
using CuraMundi.Infrastructure.DAL.Data.Configs;
using CuraMundi.Infrastructure.DAL.Repositories;
using CuraMundi.Middlewares;
using CuraMundi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddDataProtection();

builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IEmailConfirmationService, EmailConfirmationService>();
builder.Services.AddIdentityCore<User>(q =>
{
    q.User.RequireUniqueEmail = false;
    q.Password.RequiredLength = 8;
    q.Password.RequireNonAlphanumeric = false;
    q.Password.RequireLowercase = false;
    q.Password.RequireUppercase = false;
    q.SignIn.RequireConfirmedEmail = true; 
})
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager<SignInManager<User>>()
    .AddUserManager<UserManager<User>>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<EmailService>();
    
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });

    // Permet d'ajouter le "cadenas" sur les routes
    // - Impl�mentation simple (Cadenas sur toutes les routes)
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    // - Plus d'infos : 
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore?tab=readme-ov-file#add-security-definitions-and-requirements
});

builder.Services.AddAuthentication(options => 
    { 
        options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(options => 
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateIssuer = true,
            ValidateAudience = false
        };
    }
  );
builder.Services.AddCors(options =>
{
    // pas de s�curit�, tout le monde peut acc�der � l'API.
    // utile pour le d�veloppement mais � NE PAS UTILISER EN PRODUCTION
    options.AddPolicy("FFA", policy => {
        policy.AllowAnyOrigin();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });

    // Configuration de s�curit� pour le d�veloppement
    // uniquement les clients avec ces URLs sp�cifiques peuvent acc�der � l'API
    options.AddPolicy("Dev", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://demo.be");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
}
);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("Dev"); // on utilise la policy "Dev" d�finie plus haut
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
