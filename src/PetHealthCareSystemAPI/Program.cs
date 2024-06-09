using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetHealthCareSystemAPI.Middlewares;
using Repository;
using Serilog;
using Service.Utils;
using Utility.Config;
using Service.IServices;
using Service.Services;
using BusinessObject.Entities.Identity;
using Repository.Repositories;
using BusinessObject.Entities;
using PetHealthCareSystemAPI.Auth;
using BusinessObject.Mapper;
using DataAccessLayer;
using Repository.Base;
using Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add CORS policy
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
            //.AllowCredentials();
                                      
        });
});

// Add serilog and get configuration from appsettings.json
builder.Services.AddSerilog(config => { config.ReadFrom.Configuration(builder.Configuration); });

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.MigrationsAssembly(
                typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name);
            sqlServerOptionsAction.MigrationsHistoryTable("Migration");
        }));

// Add system setting from appsettings.json
var systemSettingModel = new SystemSettingModel();
builder.Configuration.GetSection("SystemSetting").Bind(systemSettingModel);
SystemSettingModel.Instance = systemSettingModel;

//Add controllers
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "AddAuthorization",
        Description = "Give me bearer token to call API here!",
        Type = SecuritySchemeType.Http
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "PetHealthCareSystem", Version = "v1" });
});
/*
builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 5;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder
                    .Configuration["JWT:SigningKey"])
        )
    };
});
*/
builder.Services.AddHttpContextAccessor();

// Add Identity
builder.Services.AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add DI
builder.Services.AddScoped<MapperlyMapper>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Add Authorization
builder.Services.AddSingleton<IAuthorizationPolicyProvider, ApiPolicyAuthorizationProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, ApiPolicyAuthorizationHandler>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = JwtUtils.GetTokenValidationParameters();
    });

// Add Authorization
builder.Services.AddAuthorization(cfg =>
{
    cfg.DefaultPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

//-----------------------------------------------------------------------------------------------
var app = builder.Build();
app.UseMiddleware<ErrorHandlerMiddleware>();

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
