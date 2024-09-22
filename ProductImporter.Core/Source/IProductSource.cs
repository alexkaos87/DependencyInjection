using ProductImporter.Model;

namespace ProductImporter.Core.Source;

public interface IProductSource
{
    Task OpenAsync();
    bool hasMoreProducts();
    Product GetNextProduct();
    void Close();
}