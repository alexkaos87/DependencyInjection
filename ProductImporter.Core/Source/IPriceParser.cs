using ProductImporter.Model;

namespace ProductImporter.Core.Source;

public interface IPriceParser
{
    Money Parse(string price);
}
