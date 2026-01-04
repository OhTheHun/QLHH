using System.ComponentModel.DataAnnotations;

namespace api_QLHH.SqlData.Models
{
    public class NhaCungCap
    {
        [Key]
        public Guid NhaCungCapId { get; set; }
        public string TenNhaCungCap { get; set; } = "";
        public string DiaChi { get; set; } = "";
        public string Email { get; set; } = "";
        public string Sdt { get; set; } = "";
        //public ICollection<PhieuNhap>? PhieuNhaps { get; set; }

    }
}
