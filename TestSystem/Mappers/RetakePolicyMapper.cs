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
            retakePolicy.RetakeInterval
        );
    }
}