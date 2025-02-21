using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Domain.Entities;
using CuraMundi.Infrastructure.DAL.Data;
using CuraMundi.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddDataProtection();

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



//builder.Services.Configure<IdentityOptions>(q =>
//{
//    q.User.RequireUniqueEmail = false;
//    q.Password.RequireNonAlphanumeric = false;
//    q.Password.RequireLowercase = false;
//    q.Password.RequireUppercase = false;
//    q.Password.RequiredLength = 8;
//    q.SignIn.RequireConfirmedEmail = true;


//});


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
