namespace api_QLHH.Core.DTOs.Responses
{
    public class ListAccountResponseDto
    {
        public Guid UserId { get; set; }
        public string? TenUser { get; set; }
        public string? Sdt { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string DiaChi { get; set; } = string.Empty;
    }
}
