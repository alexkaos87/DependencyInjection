using ProductImporter.Model;

namespace ProductImporter.Core.Transformations;

public interface IProductTransformationContext
{
    void SetProduct(Product product);
    public Product GetProduct();
    bool IsProductChanged();
}
