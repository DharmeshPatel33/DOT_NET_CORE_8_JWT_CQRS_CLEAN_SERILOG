using Application;
using Domain.Context;
using Domain.Model;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using Serilog;
using System.Text;
using Serilog.Formatting.Compact;
using APIService.Middlerwares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var appConfigurations = builder.Configuration.GetSection(AppConfigurations.SectionName).Get<AppConfigurations>();
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configurations.GetConnectionString("ConnStr")));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(appConfigurations.ConnectionStrings));



builder.Services.AddSingleton(appConfigurations);

var logger = new LoggerConfiguration()
.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
.MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Information)
.MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Information)
.MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)
.ReadFrom.Configuration(builder.Configuration)
.Enrich.FromLogContext()
.WriteTo.Console(new RenderedCompactJsonFormatter())

.CreateBootstrapLogger(); // <-- Change this line!

builder.Logging.ClearProviders();
//builder.Host.UseSerilog((context, configuration) => { configuration.ReadFrom. });
builder.Host.UseSerilog(logger);
builder.Services.AddInfratructure();
builder.Services.AddApplication();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = appConfigurations.JWTConfigurations.ValidAudience,
        ValidIssuer = appConfigurations.JWTConfigurations.ValidIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appConfigurations.JWTConfigurations.Secret))
    };
});
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
