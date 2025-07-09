namespace TestSystem.Core.Dtos;

public record EmailSettingsDto(
    string? SmtpServer,
    int SmtpPort,
    bool UseSSL,
    string? Username,
    string? Password,
    string? FromEmail,
    string? FromName
);