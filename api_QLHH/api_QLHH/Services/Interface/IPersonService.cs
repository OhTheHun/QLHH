using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Services.Interface
{
    public interface IPersonService
    {
        Task<KhachHang[]> GetKhachHangListAsync();
        Task<NhaCungCap[]> GetNhaCungCapListAsync();
        Task<UserRequestDto> RegisterAsync(AccountResponseDto acc);
        Task<UserResponseDto> LoginAsync(AccountResponseDto acc);
        Task<UserResponseDto> GetByIdAsync(Guid id);
        Task<UserRequestDto> UpdateAsync(Guid id, UserRequestDto dto);
        Task<ListAccountResponseDto[]> GetListAccountAsync();
        Task<ListAccountResponseDto> AddAccountAsync(AccountRequestDto dto);

    }
}
