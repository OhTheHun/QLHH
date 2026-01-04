using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Mapping
{
    public static class AccountMapping
    {
        public static ListAccountResponseDto ToListAccountResponseDto(this Users user)
        {
            return new ListAccountResponseDto
            {
                UserId = user.UserId,
                TenUser = user.TenUser,
                Sdt = user.Sdt,
                Email = user.Email,
                Role = user.role,
                DiaChi = user.DiaChi

            };
        }

        // DTO → Entity (ADD)
        public static Users ToUserEntity(this AccountRequestDto dto, string passwordHash)
        {
            return new Users
            {
                UserId = Guid.NewGuid(),
                TenUser = dto.TenUser,
                Sdt = dto.Sdt,
                Email = dto.Email,
                MatKhauHash = passwordHash,
                role = dto.Role,
                DiaChi = dto.DiaChi
            };
        }
    }
}
