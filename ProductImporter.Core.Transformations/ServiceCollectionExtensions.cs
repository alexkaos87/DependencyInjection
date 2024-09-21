using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Core.Transformations.Utils;
using ProductImporter.Core.Transformations;

namespace ProductImporter.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProductImporterCoreTransformations(this IServiceCollection services)
    {
        return services
            .AddSingleton<IIncrementingCounter, IncrementingCounter>() // we keep the counter for all the life time of the product importer, global increasing counter
            .AddScoped<IProductTransformationContext, ProductTransformationContext>() // scoped to a single product
            .AddScoped<IProductTransformation, NameDecapitaliser>() // scoped to a single product
            .AddScoped<IProductTransformation, CurrencyNormalizer>() // scoped to a single product
            .AddScoped<IReferenceGenerator, ReferenceGenerator>() // scoped as IDataTimeProvider dependency
            .AddScoped<IDataTimeProvider, DataTimeProvider>() // scoped to a single product
            .AddScoped<IProductTransformation, ReferenceAdder>(); // scoped to a single product
    }
}
