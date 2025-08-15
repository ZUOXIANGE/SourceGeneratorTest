global using AutoCtor;

using Microsoft.AspNetCore.HttpLogging;
using Serilog.Events;
using Serilog;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Repositories;
using Serilog.Exceptions;
using Core.Options;
using Core.Data;
using Mediator;


Console.Title = "SourceGenerator相关框架测试";

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Is(LogEventLevel.Information)
        //针对特定命名空间的日志等级
        .MinimumLevel.Override("System", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Async(writeTo =>
        {
            //写入console
            writeTo.Console();
            //写入文件
            writeTo.File(path: "log/.log", buffered: true, flushToDiskInterval: TimeSpan.FromSeconds(5),
                rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10,
                rollOnFileSizeLimit: true);
        })
        .CreateLogger();

    builder.Logging.ClearProviders();
    builder.Host.UseSerilog(Log.Logger, true);

    // Add services to the container.
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.Configure<TestOptions>(builder.Configuration);

    // 注册HTTP日志记录服务
    builder.Services.AddHttpLogging(logging =>
    {
        // 设置记录的字段
        logging.LoggingFields = HttpLoggingFields.All;

        // 设置请求和响应正文的最大记录长度（单位：字节）
        logging.RequestBodyLogLimit = 8192;
        logging.ResponseBodyLogLimit = 8192;

        // 设置是否将请求和响应日志合并为一条
        logging.CombineLogs = false;
    });
    //builder.Services.AddHttpLoggingInterceptor<MyHttpLoggingInterceptor>();

    builder.Services.AddMemoryCache();

    // 添加数据库服务
    builder.Services.AddDatabase(builder.Configuration);

    // 配置 Mediator 服务
    builder.Services.AddMediator(o =>
    {
        o.ServiceLifetime = ServiceLifetime.Scoped; // 设置 Mediator 服务的生命周期
        o.NotificationPublisherType = typeof(TaskWhenAllPublisher); // 设置通知发布器类型
    });

    // 手动注册 Mediator 处理器为作用域服务
    builder.Services.AddScoped<IRequestHandler<Core.Dtos.Mediator.CreateProductCommand, Guid>, Services.Handlers.CreateProductCommandHandler>();
    builder.Services.AddScoped<IRequestHandler<Core.Dtos.Mediator.GetProductQuery, Core.Dtos.ProductDto>, Services.Handlers.GetProductQueryHandler>();
    builder.Services.AddScoped<IRequestHandler<Core.Dtos.Mediator.GetAllProductsQuery, List<Core.Dtos.ProductDto>>, Services.Handlers.GetAllProductsQueryHandler>();
    
    // 注册通知处理器（广播）
    builder.Services.AddScoped<INotificationHandler<Core.Dtos.Mediator.ProductCreatedNotification>, Services.Handlers.ProductCreatedLogHandler>();
    builder.Services.AddScoped<INotificationHandler<Core.Dtos.Mediator.ProductCreatedNotification>, Services.Handlers.ProductCreatedEmailHandler>();
    builder.Services.AddScoped<INotificationHandler<Core.Dtos.Mediator.ProductCreatedNotification>, Services.Handlers.ProductCreatedCacheHandler>();

    // 添加自定义服务
    //builder.Services.AddScoped<IProductService, ProductService>();
    //builder.Services.AddScoped<IOrderService, OrderService>();

    //使用SourceGenerator在编译时生成注入方法
    //builder.Services.AddScoped<IOrderService, OrderService>();
    //Injectio  需要在每个类上标记
    builder.Services.AddServices();
    //ServiceScan.SourceGenerator  可以通过名称等条件过滤
    builder.Services.AddRepositories();
    //使用CustomHandler实现配置注入
    builder.Services.AddCustomOptions(builder.Configuration);

    //使用反射扫描注入
    //builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    //builder.Services.Scan(scan => scan
    //    .FromAssemblyOf<IOrderRepository>()
    //    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
    //    .AsImplementedInterfaces()
    //    .WithScopedLifetime());

    var app = builder.Build();

    // 确保数据库已创建
    await app.Services.EnsureDatabaseCreatedAsync();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpLogging();
    app.UseRouting();
    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.Information("Server Shutting down...");
    Log.CloseAndFlush();
}
