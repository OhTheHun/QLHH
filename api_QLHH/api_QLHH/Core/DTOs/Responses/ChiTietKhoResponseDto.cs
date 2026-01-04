using api_QLHH.SqlData.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace api_QLHH.Core.DTOs.Responses
{
    public class ChiTietKhoResponseDto
    {
        public Guid SanPhamId { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public Guid KhoId { get; set; }
        public string TenKho { get; set; } = string.Empty;
        public string DiaChiKho { get; set; } = string.Empty;
        public int SoLuong { get; set; }
    }
}
