//#define UseSqlTarget

#if UseSqlTarget
using Microsoft.EntityFrameworkCore;
#endif // UseSqlTarget
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.Core.Shared;
using ProductImporter.Core.Source;
using ProductImporter.Core.Target;
#if UseSqlTarget
using ProductImporter.Core.Target.EntityFramework;
#endif // UseSqlTarget
using ProductImporter.Core.Transformation;

namespace ProductImporter.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductImporterCore(this IServiceCollection services, HostBuilderContext context)
    {
        services.AddOptions<ProductSourceOptions>()
            .Configure<IConfiguration>((options, config) => config.GetSection(ProductSourceOptions.SectionName).Bind(options));
        services.AddOptions<CsvProductTargetOptions>()
            .Configure<IConfiguration>((options, config) => config.GetSection(CsvProductTargetOptions.SectionName).Bind(options));

        return services
            .AddTransient<IPriceParser, PriceParser>()
            .AddTransient<IProductSource, ProductSource>()

            .AddTransient<IProductFormatter, ProductFormatter>()
#if UseSqlTarget
            .AddTransient<IProductTarget, SqlProductTarget>()
#else
            .AddTransient<IProductTarget, CsvProductTarget>()
#endif // UseSqlTarget

            .AddTransient<ProductsManager>()

            .AddSingleton<IImportStatistics, ImportStatistics>() // singleton since need to be reused by every imported product

            .AddTransient<IProductTransformer, ProductTransformer>()

#if UseSqlTarget
            .AddDbContext<TargetContext>(options => options.UseSqlServer(context.Configuration["TargetContextConnectionString"]));
#else
            ;
#endif // UseSqlTarget
    }
}
