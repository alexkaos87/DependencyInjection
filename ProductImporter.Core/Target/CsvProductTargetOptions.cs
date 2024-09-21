namespace ProductImporter.Core.Target;

public class CsvProductTargetOptions
{
    public const string SectionName = "CsvProductTarget";

    public string TargetCsvPath { get; set; } = string.Empty;
}
