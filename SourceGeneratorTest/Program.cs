global using AutoCtor;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpLogging;
using Serilog.Events;
using Serilog;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Http.Features;
using Repositories;
using Serilog.Exceptions;
using Core.Options;
using Core.Data;
using Mediator;
using Scalar.AspNetCore;
using SourceGeneratorTest.Endpoints;
using SourceGeneratorTest.Infrastructure;
using SourceGeneratorTest.Mcp;
using Core.Dtos;
using Core.Exceptions;


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
    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

    // 上传文件大小限制与 Kestrel/表单配置保持一致
    var maxUploadSizeMB = builder.Configuration.GetValue<int?>("FileStorage:MaxSizeMB") ?? 20;
    builder.Services.Configure<FormOptions>(options =>
    {
        options.MultipartBodyLengthLimit = maxUploadSizeMB * 1024L * 1024L;
    });
    // 使用内置 OpenAPI 支持并由 Scalar 提供 UI，并补充枚举的中文描述
    builder.Services.AddOpenApi(options =>
    {
        // 把处理函数的原始返回类型包装成统一响应结构
        options.AddDocumentTransformer(ApiResultDocumentTransformer.TransformAsync);

        // 内置 OpenAPI 不会输出枚举值，也不会读取 [Description]，这里统一补齐
        options.AddSchemaTransformer((schema, context, _) =>
        {
            var type = context.JsonTypeInfo.Type;
            if (!type.IsEnum)
                return Task.CompletedTask;

            var values = new List<JsonNode>();
            var names = new JsonArray();
            var descriptions = new JsonObject();

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = Convert.ToInt64(field.GetValue(null), CultureInfo.InvariantCulture);
                var description = field.GetCustomAttribute<DescriptionAttribute>()?.Description ?? field.Name;

                values.Add(JsonValue.Create(value));
                names.Add(JsonValue.Create(field.Name));
                descriptions[value.ToString(CultureInfo.InvariantCulture)] = description;
            }

            schema.Enum = values;
            schema.AddExtension("x-enum-varnames", new JsonNodeExtension(names));
            schema.AddExtension("x-enum-descriptions", new JsonNodeExtension(descriptions));

            return Task.CompletedTask;
        });
    });
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

    //// 手动注册 Mediator 处理器为作用域服务
    //builder.Services.AddScoped<IRequestHandler<Core.Dtos.Mediator.CreateProductCommand, Guid>, Services.Handlers.CreateProductCommandHandler>();
    //builder.Services.AddScoped<IRequestHandler<Core.Dtos.Mediator.GetProductQuery, Core.Dtos.ProductDto>, Services.Handlers.GetProductQueryHandler>();
    //builder.Services.AddScoped<IRequestHandler<Core.Dtos.Mediator.GetAllProductsQuery, List<Core.Dtos.ProductDto>>, Services.Handlers.GetAllProductsQueryHandler>();

    //// 注册通知处理器（广播）
    //builder.Services.AddScoped<INotificationHandler<Core.Dtos.Mediator.ProductCreatedNotification>, Services.Handlers.ProductCreatedLogHandler>();
    //builder.Services.AddScoped<INotificationHandler<Core.Dtos.Mediator.ProductCreatedNotification>, Services.Handlers.ProductCreatedEmailHandler>();
    //builder.Services.AddScoped<INotificationHandler<Core.Dtos.Mediator.ProductCreatedNotification>, Services.Handlers.ProductCreatedCacheHandler>();

    // 添加自定义服务
    //builder.Services.AddScoped<IProductService, ProductService>();
    //builder.Services.AddScoped<IOrderService, OrderService>();

    //使用SourceGenerator在编译时生成注入方法
    //builder.Services.AddScoped<IOrderService, OrderService>();
    //Injectio  需要在每个类上标记
    builder.Services.AddServices();
    //ServiceScan.SourceGenerator  可以通过名称等条件过滤
    builder.Services.AddRepositories();
    //注册 MCP 服务：Product 相关的工具通过 Streamable HTTP 暴露给大模型客户端
    builder.Services.AddMcpServer()
        .WithHttpTransport(options => options.Stateless = true)
        .WithTools<ProductMcpTools>();
    //使用CustomHandler实现配置注入
    builder.Services.AddCustomOptions(builder.Configuration);

    //把文件存储的相对路径解析为基于内容根目录的绝对路径
    builder.Services.PostConfigure<FileStorageOptions>(options =>
    {
        if (!Path.IsPathRooted(options.RootPath))
            options.RootPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, options.RootPath));
    });

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
        app.MapOpenApi();
        var devUrl = builder.Configuration["urls"];
        app.MapScalarApiReference(options =>
        {
            options.DarkMode = true;
            options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.CSharp, ScalarClient.HttpClient);
            options.Theme = ScalarTheme.DeepSpace;
            options.ExpandAllResponses = true;
            if (!string.IsNullOrWhiteSpace(devUrl))
            {
                options.AddServer(devUrl, "开发环境");
            }
        });
    }

    app.UseHttpLogging();

    //统一异常响应：未处理异常同样返回 HTTP 200，业务状态放在 code 中
    app.UseExceptionHandler(new ExceptionHandlerOptions
    {
        ExceptionHandler = async context =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            var (code, msg) = exception switch
            {
                ApiException ex => (ex.Code, ex.Message),
                BadHttpRequestException => (StatusCodes.Status400BadRequest, "请求参数无效"),
                _ => (StatusCodes.Status500InternalServerError, "服务器内部错误")
            };

            if (exception is ApiException apiEx)
                Log.Warning("业务异常({Code}): {Message}", apiEx.Code, apiEx.Message);
            else if (exception is BadHttpRequestException badEx)
                Log.Warning(badEx, "请求参数无效");
            else
                Log.Error(exception, "未处理的异常");

            // 异常中间件默认会写入 500 状态码，这里清空后按统一响应输出 HTTP 200
            context.Response.Clear();
            await Results.Ok(ApiResult<object?>.Failure(code, msg)).ExecuteAsync(context);
        }
    });

    //使用ServiceScan.SourceGenerator在编译时扫描并映射最小API端点
    app.MapEndpoints();

    //映射 MCP 端点（Streamable HTTP）
    app.MapMcp("/mcp");

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
