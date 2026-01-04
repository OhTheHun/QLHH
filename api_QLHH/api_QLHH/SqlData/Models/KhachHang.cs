using System.ComponentModel.DataAnnotations;

namespace api_QLHH.SqlData.Models
{
    public class KhachHang
    {
        [Key]
        public Guid KhachHangId { get; set; }
        public string TenKH { get; set; } = "";
        public string DiaChi { get; set; } = "";
        public string Email { get; set; } = "";
        public string Sdt { get; set; } = "";
        //public ICollection<PhieuXuat>? PhieuXuats { get; set; }

    }
}
