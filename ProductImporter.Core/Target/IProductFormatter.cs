using ProductImporter.Model;

namespace ProductImporter.Core.Target;

public interface IProductFormatter
{
    string Format(Product product);
    string GetHeaderLine();
}
