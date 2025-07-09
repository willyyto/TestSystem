namespace TestSystem.Core.Dtos;

public record TestExportDto(
    Guid TestId,
    string Format, // "json", "xml", "qti", "gift", "pdf"
    bool IncludeAnswers,
    bool IncludeStatistics
);