using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.Shared;
using ProductImporter.Source;
using ProductImporter.Target;
using ProductImporter.Transformation;
using ProductImporter.Transformation.Transformations;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<Configuration>();

        services.AddTransient<IPriceParser, PriceParser>();
        services.AddTransient<IProductSource, ProductSource>();

        services.AddTransient<IProductFormatter, ProductFormatter>();
        services.AddTransient<IProductTarget, ProductTarget>();

        services.AddTransient<ProductImporter.ProductImporter>();

        services.AddSingleton<IImportStatistics, ImportStatistics>(); // singleton since need to be reused by every imported product

        services.AddScoped<IProductTransformationContext, ProductTransformationContext>(); // scoped to a single product
        services.AddScoped<INameDecapitaliser, NameDecapitaliser>(); // scoped to a single product
        services.AddScoped<ICurrencyNormalizer, CurrencyNormalizer>(); // scoped to a single product

        services.AddTransient<IProductTransformer, ProductTransformer>();
    })
    .Build();

var productImporter = host.Services.GetRequiredService<ProductImporter.ProductImporter>();
productImporter.Run();
