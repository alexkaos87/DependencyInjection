using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductImporter.CompositionRoot;
using ProductImporter.Core;

// Composition root of the dependencies
using var host = Host.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = true; // automatic detects dependency captivity triggering an error at initialization
    })
    // to avoid dependency captivity as DataTimeProvider in ReferenceGenerator is used as singleton instead of scoped due to the singleton nature of ReferenceGenerator
    .ConfigureServices((context, services) =>
    {
        services.AddProductImporter(context);
    })
    .Build();

// entrypoint of the program
var productManager = host.Services.GetRequiredService<ProductsManager>();
productManager.Run();
