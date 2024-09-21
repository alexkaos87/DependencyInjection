using ProductImporter.Model;

namespace ProductImporter.Core.Target;

public interface IProductTarget
{
    void Open();
    void AddProduct(Product product);
    void Close();
}
