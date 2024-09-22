using ProductImporter.Model;

namespace ProductImporter.Core.Transformation;

public class NullProductTransformer : IProductTransformer
{
    public Product ApplyTransformations(Product product) => product;
}
