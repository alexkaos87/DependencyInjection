using ProductImporter.Model;
using ProductImporter.Utils;

namespace ProductImporter.Transformation.Transformations;

public class ReferenceAdder : IReferenceAdder
{
    private readonly IProductTransformationContext _productTransformationContext;
    private readonly IReferenceGenerator _referenceGenerator;

    public ReferenceAdder(in IProductTransformationContext productTransformationContext, in IReferenceGenerator referenceGenerator)
    {
        _productTransformationContext = productTransformationContext;
        _referenceGenerator = referenceGenerator;
    }

    public void Execute()
    {
        var product = _productTransformationContext.GetProduct();

        var reference = _referenceGenerator.GetReference();

        var newProduct = new Product(product.Id, product.Name, product.Price, product.Stock, reference);

        _productTransformationContext.SetProduct(newProduct);
    }
}
