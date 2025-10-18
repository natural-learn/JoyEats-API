using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using SkyTakeOut.Admin.Middlewares;
using SkyTakeOut.Core.Autofac;
using SkyTakeOut.EntityFrameworkCore;
using System.Reflection;

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

builder.Services.AddControllers();

var app = builder.Build();

// 全局异常处理中间件
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
