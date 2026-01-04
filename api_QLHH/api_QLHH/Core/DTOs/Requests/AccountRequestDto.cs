namespace api_QLHH.Core.DTOs.Requests
{
    public class AccountRequestDto
    {
        public string? TenUser { get; set; }
        public string? Sdt { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
    }
}
