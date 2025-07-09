namespace TestSystem.Core.Dtos;

public record TestImportDto(
    string TestData, // JSON or XML test data
    string Format, // "json", "xml", "qti", "gift"
    bool OverwriteExisting,
    Guid TargetCompanyId
);