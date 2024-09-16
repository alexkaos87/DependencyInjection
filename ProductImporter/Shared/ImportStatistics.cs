using System.Text;

namespace ProductImporter.Shared;

public class ImportStatistics : IImportStatistics
{
    private int _importedCounter;
    private int _outputCounter;

    public string GetStatistics()
    {
        var buffer = new StringBuilder();
        buffer.Append($"Read a total of {_importedCounter} products from source");
        buffer.AppendLine();
        buffer.Append($"Written a total of {_outputCounter} products to target");
        
        return buffer.ToString();
    }

    public void IncrementImportCount() => ++_importedCounter;
     
    public void IncrementOutputCount() => ++_outputCounter;
}
