using Autofac;
using Autofac.Extensions.DependencyInjection;
using JoyEats.Admin.Middlewares;
using JoyEats.Common.Configs;
using JoyEats.Common.Converter;
using JoyEats.Core.Autofac;
using JoyEats.Core.Automapper;
using JoyEats.EntityFrameworkCore;
using JoyEats.EntityFrameworkCore.Interceptor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Minio;
using Serilog;
using Serilog.Events;
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
           .AddInterceptors(new AuditInterceptor())
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

// JWT
builder.Services.Configure<JwtAdminSettings>(builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtAdminSettings>();

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

// 跨域
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder.WithOrigins(new string[]
    {
            "http://localhost:80",
            "http://localhost:8096"
    })
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

// Minio
builder.Services.AddOptions<MinioSettings>()
    .Bind(builder.Configuration.GetSection("Minio"))
    .ValidateDataAnnotations()
    .Validate(config =>
    {
        if (string.IsNullOrWhiteSpace(config.Endpoint))
            throw new ArgumentException("Minio配置缺少Endpoint");
        if (string.IsNullOrWhiteSpace(config.AccessKey))
            throw new ArgumentException("Minio配置缺少AccessKey");
        if (string.IsNullOrWhiteSpace(config.SecretKey))
            throw new ArgumentException("Minio配置缺少SecretKey");
        return true;
    }, "Minio配置不完整");
var minioSettings = builder.Configuration.GetSection("Minio").Get<MinioSettings>();
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioSettings.Endpoint)
    .WithCredentials(minioSettings.AccessKey, minioSettings.SecretKey)
    .WithSSL(false)
    .Build());


var app = builder.Build();

// 全局异常处理中间件
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
