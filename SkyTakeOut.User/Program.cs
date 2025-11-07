using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using SkyTakeOut.Common.Configs;
using SkyTakeOut.Common.Converter;
using SkyTakeOut.Common.Helpers;
using SkyTakeOut.Core.Autofac;
using SkyTakeOut.Core.Automapper;
using SkyTakeOut.EntityFrameworkCore;
using SkyTakeOut.EntityFrameworkCore.Interceptor;
using SkyTakeOut.User.Middlewares;
using System.Net;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

// 日志记录
Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    .WriteTo.Async(c => c.Console())
    .WriteTo.Async(c => c.File("Logs/log.txt", rollingInterval: RollingInterval.Day))
    .CreateLogger();

builder.Host.UseSerilog();

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModule>();

    // 扫描当前程序集中的所有控制器
    var controllers = Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => typeof(ControllerBase).IsAssignableFrom(t)).ToArray();

    // 注册控制器并启用属性注入
    builder.RegisterTypes(controllers).PropertiesAutowired();
});

// EntityFrameworkCore
builder.Services.AddDbContext<AppDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("MySqlDbConnection") ?? throw new
        InvalidOperationException("数据库连接字符串获取失败！");
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31)))
           .LogTo(Console.WriteLine, LogLevel.Information)
           .EnableSensitiveDataLogging();
});


// Automapper
builder.Services.AddAutoMapper(typeof(AutomapperProfile));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // api日期统一返回格式：（"yyyy-MM-dd HH:mm:ss"）
        options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
    });

builder.Services.Configure<WeChatSettings>(builder.Configuration.GetSection("WeChat"));

// JWT
builder.Services.Configure<JwtUserSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtUserSettings>();
builder.Services.AddScoped<JWTHelper>();

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
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Headers.TryGetValue("Token", out var tokenValue)
                ? tokenValue.FirstOrDefault()
                : null;
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// 跨域
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.WithOrigins(new string[]
    {
            "http://localhost:80",
    }).AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
});

var app = builder.Build();

// 全局异常处理中间件
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
