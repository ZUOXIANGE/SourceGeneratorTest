using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;

namespace Services;

public static partial class ServicesExtensions
{
    [GenerateServiceRegistrations(
        TypeNameFilter = "*Service",
        AsImplementedInterfaces = true,
        Lifetime = ServiceLifetime.Scoped)]
    public static partial IServiceCollection AddServices(this IServiceCollection services);
}