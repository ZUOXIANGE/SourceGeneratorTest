using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System.Text.Json;
using AutoDbSetGenerators;

namespace Core.Data;

/// <summary>
/// 应用程序数据库上下文
/// </summary>
[AutoDbContext]
public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSet属性将由AutoDbSet源代码生成器自动生成

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置ProductEntity
        modelBuilder.Entity<ProductEntity>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Category).HasConversion<int>();
        });

        // 配置OrderEntity
        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.OrderType).HasConversion<int>();
            
            // 配置Address为JSON列
            entity.Property(e => e.Address)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                    v => JsonSerializer.Deserialize<AddressDto>(v, (JsonSerializerOptions)null!));
        });

        // 添加种子数据
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // 产品种子数据
        modelBuilder.Entity<ProductEntity>().HasData(
            new ProductEntity
            {
                ProductId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "iPhone 15",
                Description = "最新款苹果手机",
                Price = 7999.00m,
                Category = ProductCategory.Electronics,
                CreatedAt = DateTime.Now,
                IsAvailable = true
            },
            new ProductEntity
            {
                ProductId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "编程艺术",
                Description = "经典编程书籍",
                Price = 89.00m,
                Category = ProductCategory.Books,
                CreatedAt = DateTime.Now,
                IsAvailable = true
            }
        );

        // 订单种子数据
        modelBuilder.Entity<OrderEntity>().HasData(
            new OrderEntity
            {
                OrderId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "张三",
                Phone = "13800138000",
                OrderType = OrderType.Rental
            }
        );
    }
}