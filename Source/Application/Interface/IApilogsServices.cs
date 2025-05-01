using Shared.DTO;

namespace Application.Interface
{
    public interface IApilogsServices
    {
        Task<ApiLogDto> AddApilog(InsertApiLogDto insertApiLogDto);
    }
}