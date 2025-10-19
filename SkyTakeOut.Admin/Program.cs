using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using SkyTakeOut.Admin.Middlewares;
using SkyTakeOut.Common.Configs;
using SkyTakeOut.Common.Helpers;
using SkyTakeOut.Core.Autofac;
using SkyTakeOut.Core.Automapper;
using SkyTakeOut.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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

// JWT
builder.Services.Configure<JwtAdminOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddSingleton<JWTHelper>(_ => new JWTHelper(
        secretKey: builder.Configuration["Jwt:Secret"],
        issuer: builder.Configuration["Jwt:Issuer"],
        expireMinutes: builder.Configuration.GetValue<int>("Jwt:ExpireMinutes")));

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

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();


public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            if (DateTime.TryParse(reader.GetString(), out DateTime dateTime))
            {
                return dateTime;
            }
        }
        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}