using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Model;
using ProductImporter.Core.Shared;
using ProductImporter.Core.Transformations;

namespace ProductImporter.Core.Transformation;

public class ProductTransformer : IProductTransformer
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IWriteImportStatistics _importStatistics;

    public ProductTransformer(IServiceScopeFactory serviceScopeFactory, IWriteImportStatistics importStatistics)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _importStatistics = importStatistics;
    }

    public Product ApplyTransformations(Product product)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var transformationContext = scope.ServiceProvider.GetRequiredService<IProductTransformationContext>();
        transformationContext.SetProduct(product);

        var transformationsList = scope.ServiceProvider.GetRequiredService<IEnumerable<IProductTransformation>>();

        foreach (var transformation in transformationsList)
        {
            transformation.Execute();
        }

        if (transformationContext.IsProductChanged())
            // send statistic
            _importStatistics.IncrementTransformationCount();

        return transformationContext.GetProduct();
    }
}
