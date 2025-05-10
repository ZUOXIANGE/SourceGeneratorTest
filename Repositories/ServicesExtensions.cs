using Microsoft.Extensions.DependencyInjection;
using ServiceScan.SourceGenerator;

namespace Repositories;

public static partial class ServicesExtensions
{
    [GenerateServiceRegistrations(
        TypeNameFilter = "*Repository",
        AsImplementedInterfaces = true,
        Lifetime = ServiceLifetime.Scoped)]
    public static partial IServiceCollection AddRepositories(this IServiceCollection services);
}