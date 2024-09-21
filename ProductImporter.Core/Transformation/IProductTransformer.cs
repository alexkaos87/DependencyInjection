using ProductImporter.Model;

namespace ProductImporter.Core.Transformation
{
    public interface IProductTransformer
    {
        Product ApplyTransformations(Product product);
    }
}
