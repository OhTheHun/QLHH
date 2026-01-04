using api_QLHH.SqlData.DataContext;
using api_QLHH.SqlData.Models;
using Microsoft.EntityFrameworkCore;

namespace api_QLHH.Data.Interface
{
    public class PersonRepository(SqlContext dbContext) : IPersonRepository
    {
        private readonly SqlContext _dbContext = dbContext;

        public async Task<KhachHang[]> GetListKhachHangAsync()
        {
            return await _dbContext.KhachHang.ToArrayAsync();
        }

        public async Task<NhaCungCap[]> GetListNhaCungCapAsync()
        {
            return await _dbContext.NhaCungCap.ToArrayAsync();
        }
        public async Task<Users?> GetByEmailAsync(string email)
            => await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<Users?> GetByIdAsync(Guid id)
            => await _dbContext.Users.FindAsync(id);

        public async Task AddAsync(Users user)
            => await _dbContext.Users.AddAsync(user);

        public Task UpdateAsync(Users user)
        {
            _dbContext.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task SaveAsync()
            => await _dbContext.SaveChangesAsync();

        public async Task<Users[]> GetListAccountAsync()
        {
            return await _dbContext.Users.ToArrayAsync();
        }

        public async Task<Users> AddAccountAsync(Users user)
        {

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}
