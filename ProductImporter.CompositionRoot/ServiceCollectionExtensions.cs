using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.Core;
using ProductImporter.Core.Source;
using ProductImporter.Core.Target;

namespace ProductImporter.CompositionRoot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProductImporter(this IServiceCollection services, HostBuilderContext context)
        {
            return services
                .AddProductImporterCore(context, options => { options.SourceProductType = SourceType.CsvFile; options.TargetProductType = TargetType.CsvFile; })
                .AddProductImporterCoreTransformations();
        }
    }
}
