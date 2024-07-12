namespace TestSystem.Core.Dtos;

public record RetakePolicyDto(bool AllowRetakes, int MaxRetakes, TimeSpan RetakeInterval);