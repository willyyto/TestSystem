namespace TestSystem.Core.Dtos;

public record RetakePolicyDto(
    bool AllowRetakes,
    int MaxRetakes,
    TimeSpan RetakeInterval,
    bool RequirePasswordForRetake,
    bool ResetProgressOnRetake,
    bool ShowPreviousResults,
    decimal RetakePenalty
);