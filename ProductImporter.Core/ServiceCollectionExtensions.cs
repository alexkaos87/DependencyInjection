using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.Core.Shared;
using ProductImporter.Core.Source;
using ProductImporter.Core.Target;
using ProductImporter.Core.Target.EntityFramework;
using ProductImporter.Core.Transformation;

namespace ProductImporter.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductImporterCore(this IServiceCollection services, HostBuilderContext context, Action<ProductTargetOptions> optionsProvider)
    {
        services.AddOptions<ProductSourceOptions>()
            .Configure<IConfiguration>((options, config) => config.GetSection(ProductSourceOptions.SectionName).Bind(options));
        services.AddOptions<CsvProductTargetOptions>()
            .Configure<IConfiguration>((options, config) => config.GetSection(CsvProductTargetOptions.SectionName).Bind(options));

        var targetOptions = new ProductTargetOptions();
        optionsProvider(targetOptions);
        switch (targetOptions.Type)
        {
            case TargetType.CsvFile:
                services.AddTransient<IProductTarget, CsvProductTarget>();
                break;
            case TargetType.SqlServer:
                services
                    .AddTransient<IProductTarget, SqlProductTarget>()
                    .AddDbContext<TargetContext>(options => options.UseSqlServer(context.Configuration["TargetContextConnectionString"]));
                break;
        }

        return services
            .AddTransient<IPriceParser, PriceParser>()
            .AddTransient<IProductSource, ProductSource>()

            .AddTransient<IProductFormatter, ProductFormatter>()

            .AddTransient<ProductsManager>()

            .AddSingleton<IImportStatistics, ImportStatistics>() // singleton since need to be reused by every imported product

            .AddTransient<IProductTransformer, ProductTransformer>();
    }
}
