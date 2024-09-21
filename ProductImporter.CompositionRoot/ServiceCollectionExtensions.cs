using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.Core;

namespace ProductImporter.CompositionRoot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductImporter(this IServiceCollection services, HostBuilderContext context)
        {
            return services
                .AddProductImporterCore(context)
                .AddProductImporterCoreTransformations();
        }
    }
}
