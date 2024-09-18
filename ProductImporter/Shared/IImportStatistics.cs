﻿namespace ProductImporter.Shared;

public interface IImportStatistics
{
    void IncrementImportCount();

    void IncrementOutputCount();

    void IncrementTransformationCount();

    string GetStatistics();
}