//#define UseSqlTarget

#if UseSqlTarget
using Microsoft.EntityFrameworkCore;
#endif // UseSqlTarget
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.Core;
using ProductImporter.Core.Shared;
using ProductImporter.Core.Source;
using ProductImporter.Core.Target;
#if UseSqlTarget
using ProductImporter.Core.Target.EntityFramework;
#endif // UseSqlTarget
using ProductImporter.Core.Transformation;
using ProductImporter.Core.Transformations;
using ProductImporter.Core.Transformations.Utils;

namespace ProductImporter.CompositionRoot
{
    public static class ProductImporterCompositionRoot
    {
        public static IServiceCollection AddProductImporter(this IServiceCollection services, HostBuilderContext context)
        {
            services.AddSingleton<Configuration>(); // changing to singleton in order to have same configuration shared to whole program

            services.AddTransient<IPriceParser, PriceParser>();
            services.AddTransient<IProductSource, ProductSource>();

            services.AddTransient<IProductFormatter, ProductFormatter>();
#if UseSqlTarget
            services.AddTransient<IProductTarget, SqlProductTarget>();
#else
            services.AddTransient<IProductTarget, CsvProductTarget>();
#endif // UseSqlTarget

            services.AddTransient<ProductsManager>();

            services.AddSingleton<IImportStatistics, ImportStatistics>(); // singleton since need to be reused by every imported product

            services.AddTransient<IProductTransformer, ProductTransformer>();

            // scoped to the product
            services.AddScoped<IProductTransformationContext, ProductTransformationContext>(); // scoped to a single product
            services.AddScoped<INameDecapitaliser, NameDecapitaliser>(); // scoped to a single product
            services.AddScoped<ICurrencyNormalizer, CurrencyNormalizer>(); // scoped to a single product

            services.AddScoped<IReferenceGenerator, ReferenceGenerator>(); // scoped as IDataTimeProvider dependency
            services.AddScoped<IDataTimeProvider, DataTimeProvider>(); // scoped to a single product
            services.AddScoped<IReferenceAdder, ReferenceAdder>(); // scoped to a single product
            // --

            services.AddSingleton<IIncrementingCounter, IncrementingCounter>(); // we keep the counter for all the life time of the product importer, global increasing counter

#if UseSqlTarget
            services.AddDbContext<TargetContext>(options => options.UseSqlServer(context.Configuration["TargetContextConnectionString"]));
#endif // UseSqlTarget

            return services;
        }
    }
}
