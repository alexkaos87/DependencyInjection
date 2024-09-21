namespace ProductImporter.Model;

public class Money
{
    public const string USD = "USD";
    public const string EUR = "EUR";
    public const decimal USDToEURRate = 0.9m;

    public Money() : this(EUR, 0)
    {        
    }

    public Money(string isoCurrency, decimal amount)
    {
        IsoCurrency = isoCurrency;
        Amount = amount;
    }
    public string IsoCurrency { get; }
    public decimal Amount { get; }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Money)obj;

        return IsoCurrency == other.IsoCurrency
            && Amount == other.Amount;
    }

    public override int GetHashCode() => HashCode.Combine(IsoCurrency, Amount);
}