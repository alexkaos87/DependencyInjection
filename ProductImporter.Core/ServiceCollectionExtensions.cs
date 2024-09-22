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
    public static IServiceCollection AddProductImporterCore(this IServiceCollection services, HostBuilderContext context, Action<ProductOptions> optionsProvider)
    {
        services.AddOptions<ProductSourceOptions>()
            .Configure<IConfiguration>((options, config) => config.GetSection(ProductSourceOptions.SectionName).Bind(options));
        services.AddOptions<CsvProductTargetOptions>()
            .Configure<IConfiguration>((options, config) => config.GetSection(CsvProductTargetOptions.SectionName).Bind(options));

        var productOptions = new ProductOptions();
        optionsProvider(productOptions);
        switch (productOptions.SourceProductType)
        {
            case SourceType.CsvFile:
                services.AddTransient<IProductSource, CsvProductSource>();
                break;
            case SourceType.Http:
                services
                    .AddHttpClient<IProductSource, HttpProductSource>()
                    .ConfigureHttpClient(client => client.BaseAddress = new Uri("https://raw.githubusercontent.com/henrybeen/"));
                break;
        }

        switch (productOptions.TargetProductType)
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

        if (productOptions.ApplyTransformations)
        {
            if (productOptions.UseLazyTransformer)
            {
                services.AddTransient<Lazy<IProductTransformer>>(provider => new(() =>
                {
                    var serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
                    var importStatistics = provider.GetRequiredService<IWriteImportStatistics>();
                    return new ProductTransformer(serviceScopeFactory, importStatistics);
                }));
            }
            else
            {
                services.AddTransient<IProductTransformer, ProductTransformer>();
            }
        }
        else
        {
            services.AddTransient<IProductTransformer, NullProductTransformer>();
        }

        return services
            .AddTransient<IPriceParser, PriceParser>()
            .AddTransient<IProductFormatter, ProductFormatter>()
            .AddTransient<ProductsManager>()
            .AddSingleton<ImportStatistics>() // singleton since need to be reused by every imported product
            .AddSingleton<IWriteImportStatistics>(provider => provider.GetRequiredService<ImportStatistics>()) 
            .AddSingleton<IGetImportStatistics>(provider => provider.GetRequiredService<ImportStatistics>());
    }
}
