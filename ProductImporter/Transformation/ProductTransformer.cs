using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Model;
using ProductImporter.Shared;
using ProductImporter.Transformation.Transformations;

namespace ProductImporter.Transformation;

public class ProductTransformer : IProductTransformer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IImportStatistics _importStatistics;

    public ProductTransformer(IServiceScopeFactory serviceScopeFactory, IImportStatistics importStatistics)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _importStatistics = importStatistics;
    }

    public Product ApplyTransformations(Product product)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var transformationContext = scope.ServiceProvider.GetRequiredService<IProductTransformationContext>();
        transformationContext.SetProduct(product);

        var nameDecapitalizer = scope.ServiceProvider.GetRequiredService<INameDecapitaliser>();
        var currencyNormalizer = scope.ServiceProvider.GetRequiredService<ICurrencyNormalizer>();
        var referenceAdder = scope.ServiceProvider.GetRequiredService<IReferenceAdder>();

        nameDecapitalizer.Execute();
        currencyNormalizer.Execute();
        referenceAdder.Execute();

        if (transformationContext.IsProductChanged())
        {
            // send statistic
            _importStatistics.IncrementTransformationCount();
        }
         
        return transformationContext.GetProduct();
    }
}
