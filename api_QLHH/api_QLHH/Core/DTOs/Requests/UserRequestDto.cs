namespace api_QLHH.Core.DTOs.Requests
{
    public class UserRequestDto
    {
        public string? TenUser { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public string? HinhAnh { get; set; }
        public string? Note { get; set; }
        public string role { get; set; } = string.Empty;
    }
}
