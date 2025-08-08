# SourceGeneratorTest

一个用于演示和测试各种 .NET 源代码生成器框架的示例项目。本项目展示了如何在现代 .NET 8 应用程序中使用多种源代码生成器来提高开发效率和代码质量。

## 🚀 项目特性

- **Clean Architecture**: 采用分层架构设计，包含 Core、Repositories、Services 和 API 层
- **源代码生成器集成**: 集成多个主流的源代码生成器框架
- **RESTful API**: 提供测试用订单和产品管理 API
- **配置管理**: 使用源代码生成器自动注册配置选项
- **日志记录**: 集成 Serilog 进行结构化日志记录
- **API 文档**: 集成 Swagger/OpenAPI 文档
- **现代化测试**: 使用 TUnit 测试框架进行全面的单元测试和集成测试

## 📁 项目结构

```
SourceGeneratorTest/
├── Core/                           # 核心业务逻辑层
│   ├── Entities/                   # 实体类
│   │   ├── OrderEntity.cs         # 订单实体
│   │   └── ProductEntity.cs       # 产品实体
│   ├── Dtos/                      # 数据传输对象
│   │   ├── OrderDto.cs            # 订单DTO
│   │   ├── ProductDto.cs          # 产品DTO
│   │   ├── CreateOrderReq.cs      # 创建订单请求
│   │   ├── CreateProductReq.cs    # 创建产品请求
│   │   └── Mappers/               # 对象映射器
│   │       ├── OrderMapper.cs     # 订单映射器
│   │       ├── ProductMapper.cs   # 产品映射器
│   │       └── CloneGenerator.cs  # 深拷贝生成器
│   └── Options/                   # 配置选项
│       ├── AppSettings.cs         # 应用设置
│       ├── TestOptions.cs         # 测试配置
│       ├── OrderOptions.cs        # 订单配置
│       ├── OptionAttribute.cs     # 配置特性
│       └── ServiceCollectionExtensions.cs # 配置扩展
├── Repositories/                   # 数据访问层
│   ├── IOrderRepository.cs        # 订单仓储接口
│   ├── IProductRepository.cs      # 产品仓储接口
│   ├── Impl/                      # 仓储实现
│   │   ├── OrderRepository.cs     # 订单仓储实现
│   │   └── ProductRepository.cs   # 产品仓储实现
│   └── ServicesExtensions.cs     # 仓储注册扩展
├── Services/                      # 业务服务层
│   ├── IOrderService.cs           # 订单服务接口
│   ├── IProductService.cs         # 产品服务接口
│   ├── Impl/                      # 服务实现
│   │   ├── OrderService.cs        # 订单服务实现
│   │   └── ProductService.cs      # 产品服务实现
│   └── ServicesExtensions.cs     # 服务注册扩展
├── SourceGeneratorTest/           # Web API 层
│   ├── Controllers/               # 控制器
│   │   ├── TestController.cs      # 测试控制器
│   │   └── ProductController.cs   # 产品控制器
│   ├── Program.cs                 # 程序入口
│   └── appsettings.json          # 配置文件
└── Tests/                         # 测试项目
    ├── Data/                      # 测试数据和生成器
    │   ├── DataClass.cs           # 测试数据类
    │   ├── DataSourceGenerator.cs # 数据源生成器
    │   └── DependencyInjectionClassConstructor.cs # DI构造器
    ├── GlobalSetup.cs             # 全局测试设置
    ├── MapperTests.cs             # 映射器测试
```

## 🔧 使用的源代码生成器框架

### 1. [AutoCtor](https://github.com/distantcam/AutoCtor) - 自动构造函数注入
自动生成构造函数和依赖注入代码，减少样板代码。

**使用示例:**
```csharp
[AutoConstruct]
public partial class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;
    // 构造函数将自动生成
}
```

### 2. [Mapperly](https://github.com/riok/mapperly) - 高性能对象映射
编译时生成高性能的对象映射代码，支持深拷贝。

**使用示例:**
```csharp
[Mapper]
public partial class OrderMapper
{
    [MapperIgnoreTarget(nameof(OrderEntity.OrderId))]
    public partial OrderEntity ReqToEntity(CreateOrderReq req);
    
    [MapProperty(nameof(OrderEntity.OrderType), nameof(OrderDto.OrderTypeDesc), Use = nameof(MapOrderType))]
    public partial OrderDto EntityToDto(OrderEntity req);
}
```

### 3. [NetEscapades.EnumGenerators](https://github.com/andrewlock/NetEscapades.EnumGenerators) - 枚举扩展方法
为枚举类型生成高性能的扩展方法。

**使用示例:**
```csharp
[EnumExtensions]
public enum OrderType
{
    [Description("未知")]
    Unknown = 0,
    [Description("租车")]
    Rental = 1,
    [Description("打车")]
    Taxi = 2,
}

// 生成的扩展方法: OrderType.Rental.ToStringFast()
```

### 4. [ServiceScan.SourceGenerator](https://github.com/Dreamescaper/ServiceScan.SourceGenerator) - 编译时服务注册
编译时扫描程序集并生成服务注册代码。

**使用示例:**
```csharp
public static partial class ServicesExtensions
{
    [GenerateServiceRegistrations(
        TypeNameFilter = "*Service",
        AsImplementedInterfaces = true,
        Lifetime = ServiceLifetime.Scoped)]
    public static partial IServiceCollection AddServices(this IServiceCollection services);
}
```

### 5. 自定义配置注册生成器
使用 ServiceScan.SourceGenerator 实现的自定义配置选项注册。

**使用示例:**
```csharp
[Option(nameof(TestOptions))]
public class TestOptions
{
    public string? Name { get; set; }
}

// 自动注册: services.Configure<TestOptions>(configuration.GetSection("TestOptions"));
```

### 6. [TUnit](https://github.com/thomhurst/TUnit) - 现代化测试框架
基于源代码生成器的现代 .NET 测试框架，提供高性能和类型安全的测试体验。

**主要特性:**
- **源代码生成**: 编译时生成测试代码，提供更好的性能
- **完全异步支持**: 原生支持异步测试方法
- **并行执行**: 默认并行执行测试，提高测试速度
- **类型安全**: 编译时类型检查，避免运行时错误
- **现代语法**: 支持现代 C# 语法特性

**使用示例:**
```csharp
[Test]
public async Task CreateProduct_ShouldReturnValidGuid()
{
    // Arrange
    var request = new CreateProductReq
    {
        Name = "Test Product",
        Price = 99.99m,
        Category = ProductCategory.Electronics
    };

    // Act
    var result = await _productService.CreateProductAsync(request);

    // Assert
    await Assert.That(result).IsNotEqualTo(Guid.Empty);
}

[Test]
[Arguments("iPhone", 999.99, ProductCategory.Electronics)]
[Arguments("Book", 29.99, ProductCategory.Books)]
public async Task CreateProduct_WithDifferentCategories_ShouldReturnCorrectDescription(
    string name, decimal price, ProductCategory category)
{
    // 参数化测试示例
    var request = new CreateProductReq { Name = name, Price = price, Category = category };
    var result = await _productService.CreateProductAsync(request);
    
    await Assert.That(result).IsNotEqualTo(Guid.Empty);
}

[Test]
public async Task GetProduct_WithNonExistentId_ShouldThrowException()
{
    // 异常测试示例
    var nonExistentId = Guid.NewGuid();
    
    await Assert.That(() => _productService.GetProductAsync(nonExistentId))
        .Throws<ArgumentNullException>();
}
```

## 🔍 源代码生成器的优势

1. **编译时生成**: 所有代码在编译时生成，运行时无反射开销
2. **类型安全**: 生成的代码是强类型的，编译时检查错误
3. **性能优化**: 避免了运行时反射和动态代码生成的性能损失
4. **代码可见**: 生成的代码可以在 IDE 中查看和调试
5. **减少样板代码**: 大幅减少重复的样板代码编写


## 📚 学习资源

### 源代码生成器
- [.NET Source Generators 官方文档](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)
- [AutoCtor 使用指南](https://github.com/distantcam/AutoCtor)
- [Mapperly 映射器文档](https://mapperly.riok.app/)
- [枚举生成器文档](https://github.com/andrewlock/NetEscapades.EnumGenerators)
- [ServiceScan.SourceGenerator 文档](https://github.com/Dreamescaper/ServiceScan.SourceGenerator)

### 测试框架
- [TUnit 官方文档](https://github.com/thomhurst/TUnit)
- [TUnit 快速开始指南](https://github.com/thomhurst/TUnit/wiki/Getting-Started)

## 🤝 贡献

欢迎提交 Issue 和 Pull Request 来改进这个示例项目！
