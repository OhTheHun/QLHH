using System.ComponentModel.DataAnnotations;

namespace api_QLHH.SqlData.Models
{
    public class Users
    {
        [Key]
        public Guid UserId { get; set; }
        public string? TenUser { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public string Email { get; set; } = null!;      
        public string MatKhauHash { get; set; } = null!; 
        public string? HinhAnh { get; set; }
        public string? Username { get; set; }
        public string? Note { get; set; }
        public string? role { get; set; }

    }
}
