﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.Core;
using ProductImporter.Core.Shared;
using ProductImporter.Core.Source;
using ProductImporter.Core.Target;
using ProductImporter.Core.Target.EntityFramework;
using ProductImporter.Core.Transformation;
using ProductImporter.Core.Transformations;
using ProductImporter.Core.Transformations.Utils;

// Composition root of the dependencies
using var host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = true; // automatic detects dependency captivity triggering an error at initialization
    })
    // to avoid dependency captivity as DataTimeProvider in ReferenceGenerator is used as singleton instead of scoped due to the singleton nature of ReferenceGenerator
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<Configuration>(); // changing to singleton in order to have same configuration shared to whole program

        services.AddTransient<IPriceParser, PriceParser>();
        services.AddTransient<IProductSource, ProductSource>();

        services.AddTransient<IProductFormatter, ProductFormatter>();
        services.AddTransient<IProductTarget, CsvProductTarget>();
        //services.AddTransient<IProductTarget, SqlProductTarget>();

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

        services.AddDbContext<TargetContext>(options => options.UseSqlServer(context.Configuration["TargetContextConnectionString"]));
    })
    .Build();

// entrypoint of the program
var productManager = host.Services.GetRequiredService<ProductsManager>();
productManager.Run();
