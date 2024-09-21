namespace ProductImporter.Model;
    
public class Product
{
    public Product(Guid id, string name, Money price, int stock) : this(id, name, price, stock, string.Empty)
    {
    }

    public Product(Guid id, string name, Money price, int stock, string reference)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        Reference = reference;
    }

    public Guid Id { get; }
    public string Name { get; }
    public Money Price { get; }
    public int Stock { get; }
    public string Reference { get; }
}
