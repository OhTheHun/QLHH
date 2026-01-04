using System.ComponentModel.DataAnnotations;

namespace api_QLHH.SqlData.Models
{
    public class DanhMucSanPham
    {
        [Key]
        public Guid DanhMucId { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        //public ICollection<SanPham>? SanPhams { get; set; }
    }
}
