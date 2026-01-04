namespace api_QLHH.Core.DTOs.Responses
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = "";
        public string? TenUser { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }
        public string? HinhAnh { get; set; }
        public string? Note { get; set; }
        public string role { get; set; } = string.Empty;
    }
}
