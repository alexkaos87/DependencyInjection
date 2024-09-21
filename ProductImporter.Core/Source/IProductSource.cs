using ProductImporter.Model;

namespace ProductImporter.Core.Source;

public interface IProductSource
{
    void Open();
    bool hasMoreProducts();
    Product GetNextProduct();
    void Close();
}