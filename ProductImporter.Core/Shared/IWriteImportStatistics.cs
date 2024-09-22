namespace ProductImporter.Core.Shared;

public interface IWriteImportStatistics
{
    void IncrementImportCount();

    void IncrementOutputCount();

    void IncrementTransformationCount();
}