using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;

namespace Core.Options;

public static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加自定义配置
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    [GenerateServiceRegistrations(AttributeFilter = typeof(OptionAttribute), CustomHandler = nameof(AddOption))]
    public static partial IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration);

    private static void AddOption<T>(IServiceCollection services, IConfiguration configuration) where T : class
    {
        var sectionKey = typeof(T).GetCustomAttribute<OptionAttribute>()?.Section;
        var section = sectionKey is null ? configuration : configuration.GetSection(sectionKey);
        services.Configure<T>(section);
    }
}