using api_QLHH.SqlData.Models;
using FluentValidation;

namespace api_QLHH.FluentValidation
{
    public class PersonValidator
    {
    }
    public class KhachHangValidator : AbstractValidator<KhachHang>
    {
        public KhachHangValidator()
        {
            RuleFor(x => x.TenKH)
                .NotEmpty().WithMessage("Tên khách hàng không được để trống.")
                .MaximumLength(100).WithMessage("Tên khách hàng tối đa 100 ký tự.");

            RuleFor(x => x.Sdt)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^\d{10,11}$").WithMessage("Số điện thoại phải từ 10–11 số.");

            RuleFor(x => x.DiaChi)
                .NotEmpty().WithMessage("Địa chỉ không được để trống.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email không hợp lệ.");
        }
    }

    public class NhaCungCapValidator : AbstractValidator<NhaCungCap>
    {
        public NhaCungCapValidator()
        {
            RuleFor(x => x.TenNhaCungCap)
                .NotEmpty().WithMessage("Tên nhà cung cấp không được để trống.")
                .MaximumLength(100).WithMessage("Tên nhà cung cấp tối đa 100 ký tự.");

            RuleFor(x => x.Sdt)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Matches(@"^\d{10,11}$").WithMessage("Số điện thoại phải từ 10–11 số.");

            RuleFor(x => x.DiaChi)
                .NotEmpty().WithMessage("Địa chỉ không được để trống.");

            RuleFor(x => x.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email không hợp lệ.");
        }
    }
}
