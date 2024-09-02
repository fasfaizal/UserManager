using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManager.Common.Configs;
using UserManager.Common.Interfaces.Repos;
using UserManager.Common.Interfaces.Services;
using UserManager.Repo.Extensions;
using UserManager.Repo.Repos;
using UserManager.Services.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepo, UsersRepo>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserDetailsRepo, UserDetailsRepo>();

//Add Db Context
builder.Services.AddDBService(builder.Configuration.GetConnectionString("DbConnectionString"));

//Add configurations
builder.Services.Configure<JwtConfigurations>(builder.Configuration.GetSection("JwtConfigurations"));

//Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtConfigurations:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtConfigurations:Secret"]!)),
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
