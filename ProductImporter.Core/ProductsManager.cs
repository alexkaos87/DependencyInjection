using ProductImporter.Core.Shared;
using ProductImporter.Core.Source;
using ProductImporter.Core.Target;
using ProductImporter.Core.Transformation;

namespace ProductImporter.Core;

public class ProductsManager
{
    private readonly IProductSource _productSource;
    private readonly IProductTransformer _productTransformer;
    private readonly IProductTarget _productTarget;
    private readonly IGetImportStatistics _importStatistics;

    public ProductsManager(IProductSource productSource, IProductTransformer productTransformer, IProductTarget productTarget, IGetImportStatistics importStatistics)
    {
        _productSource = productSource;
        _productTransformer = productTransformer;
        _productTarget = productTarget;
        _importStatistics = importStatistics;
    }

    public async Task RunAsync()
    {
        await _productSource.OpenAsync();
        _productTarget.Open();

        while (_productSource.hasMoreProducts())
        {
            var product = _productSource.GetNextProduct();

            var transformedProduct = _productTransformer.ApplyTransformations(product);

            _productTarget.AddProduct(transformedProduct);
        }

        _productSource.Close();
        _productTarget.Close();

        Console.WriteLine("Importing complete!");
        Console.WriteLine(_importStatistics.GetStatistics());
    }
}