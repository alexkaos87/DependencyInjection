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
    private readonly IImportStatistics _importStatistics;

    public ProductsManager(IProductSource productSource, IProductTransformer productTransformer, IProductTarget productTarget, IImportStatistics importStatistics)
    {
        _productSource = productSource;
        _productTransformer = productTransformer;
        _productTarget = productTarget;
        _importStatistics = importStatistics;
    }

    public void Run()
    {
        _productSource.Open();
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