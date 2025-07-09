using TestSystem.Core.Dtos;
using TestSystem.Core.Entities;

namespace TestSystem.Mappers;

public static class RetakePolicyMapper
{
    public static RetakePolicyDto MapToRetakePolicyDto(this RetakePolicy retakePolicy)
    {
        return new RetakePolicyDto(
            retakePolicy.AllowRetakes,
            retakePolicy.MaxRetakes,
            retakePolicy.RetakeInterval,
            retakePolicy.RequirePasswordForRetake,
            retakePolicy.ResetProgressOnRetake,
            retakePolicy.ShowPreviousResults,
            retakePolicy.RetakePenalty
        );
    }

    public static RetakePolicy MapToRetakePolicy(this CreateRetakePolicyDto dto)
    {
        return new RetakePolicy
        {
            AllowRetakes = dto.AllowRetakes,
            MaxRetakes = dto.MaxRetakes,
            RetakeInterval = dto.RetakeInterval,
            RequirePasswordForRetake = dto.RequirePasswordForRetake,
            ResetProgressOnRetake = dto.ResetProgressOnRetake,
            ShowPreviousResults = dto.ShowPreviousResults,
            RetakePenalty = dto.RetakePenalty
        };
    }
}