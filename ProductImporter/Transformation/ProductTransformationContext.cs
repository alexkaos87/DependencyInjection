using ProductImporter.Model;

namespace ProductImporter.Transformation;

public class ProductTransformationContext : IProductTransformationContext
{
    private Product? _initialProduct;
    private Product? _product;

    public Product GetProduct()
    {
        if (_product == null)
        {
            throw new InvalidOperationException("Cant get the product before setting it");
        }

        return _product;
    }

    public bool IsProductChanged() => _product != null && _initialProduct != null && !_initialProduct.Equals(_product);

    public void SetProduct(Product product)
    {
        _product = product;
        _initialProduct ??= product;
    }
}
