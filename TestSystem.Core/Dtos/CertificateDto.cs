namespace TestSystem.Core.Dtos;

public record CertificateDto(
    Guid Id,
    Guid TestResultId,
    string RecipientName,
    string TestName,
    int Score,
    DateTime CompletedDate,
    DateTime IssuedDate,
    string CertificateNumber,
    string? CertificateUrl
);
