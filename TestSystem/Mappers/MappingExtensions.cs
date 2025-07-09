using TestSystem.Core.Dtos;

namespace TestSystem.Mappers;

public static class MappingExtensions
{
    public static PagedResultDto<TDto> MapToPagedResult<TEntity, TDto>(
        this PagedResultDto<TEntity> pagedResult,
        Func<TEntity, TDto> mapper)
    {
        return new PagedResultDto<TDto>(
            pagedResult.Items.Select(mapper).ToList(),
            pagedResult.TotalCount,
            pagedResult.Page,
            pagedResult.PageSize,
            pagedResult.TotalPages
        );
    }

    public static ApiResponseDto<T> ToSuccessResponse<T>(this T data, string? message = null)
    {
        return new ApiResponseDto<T>(true, data, message, null, 200);
    }

    public static ApiResponseDto<T> ToErrorResponse<T>(string message, List<string>? errors = null, int statusCode = 400)
    {
        return new ApiResponseDto<T>(false, default(T), message, errors, statusCode);
    }
}