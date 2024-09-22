using ProductImporter.Core.Shared;
using ProductImporter.Model;
using System.Text.Json;

namespace ProductImporter.Core.Source;

public class HttpProductSource : IProductSource
{
    private readonly HttpClient _httpClient;
    private readonly IImportStatistics _importStatistics;

    private readonly Queue<Product> _cachingProducts = new();

    public HttpProductSource(HttpClient httpClient, IImportStatistics importStatistics)
    {
        _httpClient = httpClient;
        _importStatistics = importStatistics;
    }

    public void Close()
    { }

    public Product GetNextProduct()
    {
        var product = _cachingProducts.Dequeue();
        _importStatistics.IncrementImportCount();
        return product;
    }

    public bool hasMoreProducts() => _cachingProducts.Any();

    public async Task OpenAsync()
    {
        using var productStream = await _httpClient.GetStreamAsync("ps-di-files/main/products.json");
        var products = await JsonSerializer.DeserializeAsync<Product[]>(productStream);

        if (products != null)
        {
            foreach (var product in products)
            {
                _cachingProducts.Enqueue(product);
            }
        }
    }
}
