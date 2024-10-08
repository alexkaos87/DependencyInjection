﻿using ProductImporter.Core.Source;
using ProductImporter.Core.Target;

namespace ProductImporter.Core;

public class ProductOptions
{
    public SourceType SourceProductType { get; set; } = SourceType.CsvFile;
    public TargetType TargetProductType { get; set; } = TargetType.CsvFile;
    public bool ApplyTransformations { get; set; } = true;
    public bool UseLazyTransformer { get; set; } = false;
}
