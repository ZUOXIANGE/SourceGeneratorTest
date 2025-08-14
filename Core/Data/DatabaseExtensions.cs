using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Core.Data;

/// <summary>
/// 数据库配置扩展
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// 添加数据库服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns></returns>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Data Source=app.db";

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
            // 启用敏感数据日志记录（仅在开发环境）
            options.EnableSensitiveDataLogging();
            // 启用详细错误
            options.EnableDetailedErrors();
        });

        return services;
    }

    /// <summary>
    /// 确保数据库已创建
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <returns></returns>
    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // 确保数据库已创建
        await context.Database.EnsureCreatedAsync();
    }

    /// <summary>
    /// 应用数据库迁移
    /// </summary>
    /// <param name="serviceProvider">服务提供者</param>
    /// <returns></returns>
    public static async Task MigrateDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        // 应用待处理的迁移
        await context.Database.MigrateAsync();
    }
}