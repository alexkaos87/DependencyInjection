using ProductImporter.Core.Transformations.Utils;
using ProductImporter.Model;

namespace ProductImporter.Core.Transformations;

public class ReferenceAdder : IReferenceAdder
{
    private readonly IProductTransformationContext _productTransformationContext;
    private readonly IReferenceGenerator _referenceGenerator;

    public ReferenceAdder(IProductTransformationContext productTransformationContext, IReferenceGenerator referenceGenerator)
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
