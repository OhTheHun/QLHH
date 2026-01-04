using api_QLHH.Core.DTOs.Responses;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Data.Interface
{
    public interface IPersonRepository
    {
        Task<KhachHang[]> GetListKhachHangAsync();
        Task<NhaCungCap[]> GetListNhaCungCapAsync();
        Task<Users?> GetByEmailAsync(string email);
        Task<Users?> GetByIdAsync(Guid id);
        Task AddAsync(Users user);
        Task UpdateAsync(Users user);
        Task SaveAsync();
        Task<Users[]> GetListAccountAsync();
        Task<Users> AddAccountAsync(Users user);


    }
}
