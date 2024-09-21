using ProductImporter.Shared;
using ProductImporter.Model;
using Microsoft.Extensions.DependencyInjection;
using ProductImporter.Target.EntityFramework;

namespace ProductImporter.Target;

public class SqlProductTarget : IProductTarget
{
    private readonly IImportStatistics _importStatistics;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SqlProductTarget(IServiceScopeFactory serviceScopeFactory, IImportStatistics importStatistics)
    {
        _importStatistics = importStatistics;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public void Open()
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TargetContext>();

        context.Database.EnsureCreated();
    }

    public void AddProduct(Product product)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<TargetContext>();

        _importStatistics.IncrementOutputCount();

        context.Products.Add(product);
        context.SaveChanges();
    }

    public void Close()
    {
    }
}
