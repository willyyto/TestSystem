namespace TestSystem.Core.Dtos;

public record CreateRetakePolicyDto(
    bool AllowRetakes,
    int MaxRetakes,
    TimeSpan RetakeInterval,
    bool RequirePasswordForRetake,
    bool ResetProgressOnRetake,
    bool ShowPreviousResults,
    decimal RetakePenalty
);