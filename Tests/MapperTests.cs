using Core.Dtos;
using Core.Dtos.Mappers;
using Core.Entities;

namespace Tests;

/// <summary>
/// 映射器测试类
/// </summary>
public class MapperTests
{
    [Test]
    [DisplayName("产品请求到实体映射应该正确")]
    public async Task ProductMapper_ReqToEntity_ShouldMapCorrectly()
    {
        // Arrange
        var mapper = new ProductMapper();
        var request = new CreateProductReq
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Category = ProductCategory.Electronics,
            IsAvailable = true
        };

        // Act
        var entity = mapper.ReqToEntity(request);

        // Assert
        await Assert.That(entity.Name).IsEqualTo("Test Product");
        await Assert.That(entity.Description).IsEqualTo("Test Description");
        await Assert.That(entity.Price).IsEqualTo(99.99m);
        await Assert.That(entity.Category).IsEqualTo(ProductCategory.Electronics);
        await Assert.That(entity.IsAvailable).IsTrue();
        await Assert.That(entity.ProductId).IsEqualTo(Guid.Empty); // Should be ignored
    }

    [Test]
    [DisplayName("产品实体到DTO映射应该正确")]
    public async Task ProductMapper_EntityToDto_ShouldMapCorrectly()
    {
        // Arrange
        var mapper = new ProductMapper();
        var entity = new ProductEntity
        {
            ProductId = Guid.NewGuid(),
            Name = "Test Product",
            Description = "Test Description",
            Price = 99.99m,
            Category = ProductCategory.Electronics,
            IsAvailable = true,
            CreatedAt = DateTime.Now
        };

        // Act
        var dto = mapper.EntityToDto(entity);

        // Assert
        await Assert.That(dto.ProductId).IsEqualTo(entity.ProductId);
        await Assert.That(dto.Name).IsEqualTo("Test Product");
        await Assert.That(dto.Description).IsEqualTo("Test Description");
        await Assert.That(dto.Price).IsEqualTo(99.99m);
        await Assert.That(dto.CategoryDesc).IsEqualTo("电子产品"); // Mapped description
        await Assert.That(dto.IsAvailable).IsTrue();
        await Assert.That(dto.CreatedAt).IsEqualTo(entity.CreatedAt);
    }

    [Test]
    [Arguments(ProductCategory.Electronics, "电子产品")]
    [Arguments(ProductCategory.Clothing, "服装")]
    [Arguments(ProductCategory.Food, "食品")]
    [Arguments(ProductCategory.Books, "图书")]
    [Arguments(ProductCategory.Uncategorized, "未分类")]
    [DisplayName("产品类别映射应该正确")]
    public async Task ProductMapper_CategoryMapping_ShouldBeCorrect(
        ProductCategory category, string expectedDescription)
    {
        // Arrange
        var mapper = new ProductMapper();
        var entity = new ProductEntity
        {
            ProductId = Guid.NewGuid(),
            Name = "Test Product",
            Category = category,
            Price = 100m,
            IsAvailable = true,
            CreatedAt = DateTime.Now
        };

        // Act
        var dto = mapper.EntityToDto(entity);

        // Assert
        await Assert.That(dto.CategoryDesc).IsEqualTo(expectedDescription);
    }

    [Test]
    [DisplayName("订单请求到实体映射应该正确")]
    public async Task OrderMapper_ReqToEntity_ShouldMapCorrectly()
    {
        // Arrange
        var mapper = new OrderMapper();
        var request = new CreateOrderReq
        {
            Name = "张三",
            Phone = "13800138000",
            Address = new AddressDto
            {
                City = "北京",
                Detail = "朝阳区某某街道"
            },
            OrderType = OrderType.Rental
        };

        // Act
        var entity = mapper.ReqToEntity(request);

        // Assert
        await Assert.That(entity.Name).IsEqualTo("张三");
        await Assert.That(entity.Phone).IsEqualTo("13800138000");
        await Assert.That(entity.Address?.City).IsEqualTo("北京");
        await Assert.That(entity.Address?.Detail).IsEqualTo("朝阳区某某街道");
        await Assert.That(entity.OrderType).IsEqualTo(OrderType.Rental);
        await Assert.That(entity.OrderId).IsEqualTo(Guid.Empty); // Should be ignored
    }

    [Test]
    [DisplayName("订单实体到DTO映射应该正确")]
    public async Task OrderMapper_EntityToDto_ShouldMapCorrectly()
    {
        // Arrange
        var mapper = new OrderMapper();
        var entity = new OrderEntity
        {
            OrderId = Guid.NewGuid(),
            Name = "李四",
            Phone = "13900139000",
            Address = new AddressDto
            {
                City = "上海",
                Detail = "浦东新区某某路"
            },
            OrderType = OrderType.Taxi
        };

        // Act
        var dto = mapper.EntityToDto(entity);

        // Assert
        await Assert.That(dto.OrderId).IsEqualTo(entity.OrderId);
        await Assert.That(dto.Name).IsEqualTo("李四");
        await Assert.That(dto.Phone).IsEqualTo("13900139000");
        await Assert.That(dto.Address?.City).IsEqualTo("上海");
        await Assert.That(dto.OrderTypeDesc).IsEqualTo("打车"); // Mapped description
    }

    [Test]
    [Arguments(OrderType.Rental, "租车")]
    [Arguments(OrderType.Taxi, "打车")]
    [Arguments(OrderType.Unknown, "未知")]
    [DisplayName("订单类型映射应该正确")]
    public async Task OrderMapper_OrderTypeMapping_ShouldBeCorrect(
        OrderType orderType, string expectedDescription)
    {
        // Arrange
        var mapper = new OrderMapper();
        var entity = new OrderEntity
        {
            OrderId = Guid.NewGuid(),
            Name = "测试用户",
            Phone = "13800138000",
            OrderType = orderType
        };

        // Act
        var dto = mapper.EntityToDto(entity);

        // Assert
        await Assert.That(dto.OrderTypeDesc).IsEqualTo(expectedDescription);
    }

    [Test]
    [DisplayName("静态映射器应该工作正常")]
    public async Task StaticMapper_ShouldWorkCorrectly()
    {
        // Arrange
        var request = new CreateProductReq
        {
            Name = "Static Test Product",
            Description = "Static Test Description",
            Price = 199.99m,
            Category = ProductCategory.Books,
            IsAvailable = false
        };

        // Act
        var entity = request.ToEntity();

        // Assert
        await Assert.That(entity.Name).IsEqualTo("Static Test Product");
        await Assert.That(entity.Description).IsEqualTo("Static Test Description");
        await Assert.That(entity.Price).IsEqualTo(199.99m);
        await Assert.That(entity.Category).IsEqualTo(ProductCategory.Books);
        await Assert.That(entity.IsAvailable).IsFalse();
    }
}