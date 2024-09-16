using ProductImporter.Shared;
using ProductImporter.Source;
using ProductImporter.Target;

var configuration = new Configuration();

var priceParser = new PriceParser();
var productSource = new ProductSource(configuration, priceParser);

var productFormatter = new ProductFormatter();
var productTarget = new ProductTarget(configuration, productFormatter);

var productImporter = new ProductImporter.ProductImporter(productSource, productTarget);
productImporter.Run();
