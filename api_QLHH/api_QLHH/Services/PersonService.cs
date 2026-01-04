using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Data.Interface;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;
using api_QLHH.Mapping;
using BCrypt.Net;

namespace api_QLHH.Services
{
    public class PersonService(IPersonRepository personRepository) : IPersonService
    {
        private readonly IPersonRepository _personRepository = personRepository;

        public async Task<UserResponseDto> GetByIdAsync(Guid id)
        {
            var user = await _personRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User không tồn tại");

            return MapToDto(user);
        }

        public async Task<KhachHang[]> GetKhachHangListAsync()
        {
            return await _personRepository.GetListKhachHangAsync();
        }

        public async Task<NhaCungCap[]> GetNhaCungCapListAsync()
        {
            return await _personRepository.GetListNhaCungCapAsync();
        }

        public async Task<UserResponseDto> LoginAsync(AccountResponseDto acc)
        {
            var user = await _personRepository.GetByEmailAsync(acc.Email);
            if (user == null)
                throw new Exception("Sai email hoặc mật khẩu");

            if (!BCrypt.Net.BCrypt.Verify(acc.Password, user.MatKhauHash))
                throw new Exception("Sai email hoặc mật khẩu");

            return MapToDto(user);
        }

        public async Task<UserRequestDto> RegisterAsync(AccountResponseDto acc)
        {
            var existingUser = await _personRepository.GetByEmailAsync(acc.Email);
            if (existingUser != null)
                throw new Exception("Email đã tồn tại");
            var user = new Users
            {
                UserId = Guid.NewGuid(),
                Email = acc.Email,
                MatKhauHash = BCrypt.Net.BCrypt.HashPassword(acc.Password)
            };

            await _personRepository.AddAsync(user);
            await _personRepository.SaveAsync();

            return Tranform(user);
        }

        public async Task<UserRequestDto> UpdateAsync(Guid id, UserRequestDto dto)
        {
            var user = await _personRepository.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User không tồn tại");
            user.TenUser = dto.TenUser;
            user.DiaChi = dto.DiaChi;
            user.Sdt = dto.Sdt;
            user.HinhAnh = dto.HinhAnh;

            await _personRepository.UpdateAsync(user);
            await _personRepository.SaveAsync();

            return Tranform(user);
        }
        private static UserResponseDto MapToDto(Users user)
        {
            return new UserResponseDto
            {
                UserId = user.UserId,
                Email = user.Email,
                TenUser = user.TenUser,
                DiaChi = user.DiaChi,
                Sdt = user.Sdt,
                HinhAnh = user.HinhAnh,
                Note = user.Note,
                role =  user.role,
            };
        }
        private static UserRequestDto Tranform(Users user)
        {
            return new UserRequestDto
            {
                TenUser = user.TenUser,
                DiaChi = user.DiaChi,
                Sdt = user.Sdt,
                HinhAnh = user.HinhAnh,
                Note = user.Note,
                role = user.role,
            };
        }

        public async Task<ListAccountResponseDto[]> GetListAccountAsync()
        {
            var users = await _personRepository.GetListAccountAsync();

            return users
                .Select(u => u.ToListAccountResponseDto())
                .ToArray();

        }

        public async Task<ListAccountResponseDto> AddAccountAsync(AccountRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email không được để trống");

            if (string.IsNullOrWhiteSpace(dto.Role))
                throw new ArgumentException("Role không được để trống");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");

            var entity = dto.ToUserEntity(passwordHash);

            var createdUser = await _personRepository.AddAccountAsync(entity);
            return createdUser.ToListAccountResponseDto();
        }
    }
}
