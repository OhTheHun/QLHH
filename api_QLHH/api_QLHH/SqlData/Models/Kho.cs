using System.ComponentModel.DataAnnotations;

namespace api_QLHH.SqlData.Models
{
    public class Kho
    {
        [Key]
        public Guid KhoId { get; set; }
        public string TenKho { get; set; } = "";
        public string DiaChi { get; set; } = "";

        public ICollection<ChiTietKho>? ChiTietKhos { get; set; }
    }
}
