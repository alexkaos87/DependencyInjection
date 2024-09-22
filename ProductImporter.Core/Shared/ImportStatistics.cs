using System.Text;

namespace ProductImporter.Core.Shared;

public class ImportStatistics : IWriteImportStatistics, IGetImportStatistics
{
    private int _importedCounter;
    private int _outputCounter;
    private int _transformationCounter;

    public string GetStatistics()
    {
        var buffer = new StringBuilder();
        buffer.Append($"Read a total of {_importedCounter} products from source");
        buffer.AppendLine();
        buffer.Append($"Transformed a total of {_transformationCounter} products");
        buffer.AppendLine();
        buffer.Append($"Written a total of {_outputCounter} products to target");

        return buffer.ToString();
    }

    public void IncrementImportCount() => ++_importedCounter;

    public void IncrementOutputCount() => ++_outputCounter;

    public void IncrementTransformationCount() => ++_transformationCounter;
}
