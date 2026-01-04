using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.FluentValidation;
using api_QLHH.Handlers.Command;
using api_QLHH.Handlers.Commands;
using api_QLHH.Handlers.Queries;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace api_QLHH.Controllers
{
    [Route("api")]
    [ApiController]
    public class Controller : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IProductService _productService;
        private readonly RunGetKhachHangQueryHandler _getKhachHangQueryHandler;
        private readonly RunGetNhaCungCapQueryHandler _getNhaCungCapQueryHandler;
        private readonly RunGetDanhMucSanPhamQueryHandler _getDanhMucSanPhamQueryHandler;
        private readonly RunGetSanPhamQueryHandler _getSanPhamQueryHandler;
        private readonly RunGetKhoQueryHandler _getKhoQueryHandler;
        private readonly RunGetChiTietKhoQueryHandler _getChiTietKhoQueryHandler;
        private readonly RegisterCommandHandler _registerCommandHandler;
        private readonly UpdateUserCommandHandler _updateUserCommandHandler;
        private readonly GetUserByIdQueryHandler _getUserByIdQueryHandler;

        public Controller(
            IPersonService personService,
            IProductService productService,
            RunGetKhachHangQueryHandler getKhachHangQueryHandler,
            RunGetNhaCungCapQueryHandler getNhaCungCapQueryHandler,
            RunGetDanhMucSanPhamQueryHandler getDanhMucSanPhamQueryHandler,
            RunGetSanPhamQueryHandler getSanPhamQueryHandler,
            RunGetKhoQueryHandler getKhoQueryHandler,
            RunGetChiTietKhoQueryHandler getChiTietKhoQueryHandler,
            RegisterCommandHandler registerCommandHandler,
            UpdateUserCommandHandler updateUserCommandHandler,
            GetUserByIdQueryHandler getUserByIdQueryHandler

            )
        {

            // service
            _personService = personService;
            _productService = productService;

            //Handler
            _getChiTietKhoQueryHandler = getChiTietKhoQueryHandler;
            _getKhoQueryHandler = getKhoQueryHandler;
            _getKhachHangQueryHandler = getKhachHangQueryHandler;
            _getNhaCungCapQueryHandler = getNhaCungCapQueryHandler;
            _getDanhMucSanPhamQueryHandler = getDanhMucSanPhamQueryHandler;
            _getSanPhamQueryHandler = getSanPhamQueryHandler;
            _registerCommandHandler = registerCommandHandler;
            _updateUserCommandHandler = updateUserCommandHandler;
            _getUserByIdQueryHandler = getUserByIdQueryHandler;
        }

        [HttpGet("person/KhachHang")]
        public async Task<ActionResult<KhachHang[]>> GetList()
        {
            try
            {
                var entity = await _getKhachHangQueryHandler.Handle();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet("person/Nhacungcap")]
        public async Task<ActionResult<NhaCungCap[]>> GetNhaCungCapListAsync()
        {
            try
            {
                var entity = await _getNhaCungCapQueryHandler.Handle();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet("products/DanhMuc")]
        public async Task<ActionResult<DanhMucSanPham[]>> GetListDanhMucSanPhamAsync()
        {
            try
            {
                var entity = await _getDanhMucSanPhamQueryHandler.Handle();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        [HttpGet("products/SanPham")]
        public async Task<ActionResult<SanPham[]>> GetListSanPhamAsync()
        {
            try
            {
                var entity = await _getSanPhamQueryHandler.Handle();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        [HttpGet("products/Kho")]
        public async Task<ActionResult<Kho[]>> GetKhoAsync()
        {
            try
            {
                var entity = await _getKhoQueryHandler.Handle();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        [HttpGet("products/chi-tiet-kho")]
        public async Task<ActionResult<Kho[]>> GetChiTietKhoAsync()
        {
            try
            {
                var entity = await _getChiTietKhoQueryHandler.Handle();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        [HttpPost("person/register")]
        public async Task<ActionResult<Users>> Register([FromBody] AccountResponseDto dto)
        {
            try
            {
                var user = await _registerCommandHandler.Handle(dto);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("person/login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody] AccountResponseDto dto)
        {
            try
            {
                var user = await _personService.LoginAsync(dto);
                if (user == null)
                    return Unauthorized(new { error = "Email hoặc mật khẩu không đúng" });
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("person/{id}")]
        public async Task<ActionResult<UserResponseDto>> Update(Guid id, [FromBody] UserRequestDto dto)
        {
            try
            {
                var updatedUser = await _updateUserCommandHandler.Handle(id, dto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("person/{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUserById(Guid id)
        {
            try
            {
                var user = await _getUserByIdQueryHandler.Handle(id);
                if (user == null) return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        [HttpGet("product/history")]
        public async Task<ActionResult<LichSuNhapXuatResponseDto>> GetLichSuNhapXuat()
        {
            try
            {
                var result = await _productService.GetLichSuNhapXuatAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }

        }
        [HttpPost("product/phieu-nhap")]
        public async Task<ActionResult<CreatePhieuNhapRequestDto>> CreatePhieuNhapAsync(CreatePhieuNhapRequestDto request)
        {
            try
            {
                var phieuNhapId = await _productService.CreatePhieuNhapAsync(request);
                return Ok(new { phieuNhapId });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }

        }
        [HttpGet("product/report")]
        public async Task<ActionResult<BaoCaoResponseDto>> GetBaoCaoAsync([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            try
            {
            return Ok(await _productService.GetBaoCaoAsync(fromDate, toDate));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet("products/summary")]
        public async Task<ActionResult<DashBoardResponseDto>> GetDashBoardSummaryAsync([FromQuery] DateTime month)
        {
            try
            {
                return Ok(await _productService.GetSummaryAsync(month));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet("products/top-products")]
        public async Task<ActionResult<List<SanPhamBanChayResponseDto>>> GetTopProductsAsync()
        {
            try
            {
                return Ok(await _productService.GetTopProductsAsync());

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpGet("products/low-stock")]
        public async Task<ActionResult<List<SanPhamTonResponseDto>>> GetLowStockProductsAsync()
        {
            try
            {
                return Ok(await _productService.GetLowStockProductsAsync());

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPost("products/chi-tiet-kho")]
        public async Task<ActionResult<ChiTietKhoRequestDto>> AddChiTietKhoAsync([FromBody] ChiTietKhoRequestDto dto)
        {
            try
            {
                await _productService.AddChiTietKhoAsync(dto);
                return Ok(dto);
            }
                catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPut("products/chi-tiet-kho")]
        public async Task<ActionResult<ChiTietKhoRequestDto>> UpdateChiTietKhoAsync([FromBody] ChiTietKhoRequestDto dto)
        {
            try
            { 
                await _productService.UpdateChiTietKhoAsync(dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpDelete("products/chi-tiet-kho{khoId}/{sanPhamId}")]
        public async Task<ActionResult<ChiTietKhoRequestDto>> DeleteChiTietKhoAsync(Guid khoId, Guid sanPhamId)
        {
            try
            {
                await _productService.DeleteChiTietKhoAsync(khoId, sanPhamId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }
        [HttpGet("person/account")]
        public async Task<ActionResult<ListAccountResponseDto[]>> GetListAccountAsync()
        {
            try
            {
                var result = await _personService.GetListAccountAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { error = ex.Message }
                );
            }
        }

        [HttpPost("person/account")]
        public async Task<ActionResult<ListAccountResponseDto>> AddAccountAsync([FromBody] AccountRequestDto dto)
        {
            try
            {
                var result = await _personService.AddAccountAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { error = ex.Message }
                );
            }
        }
        [HttpGet("products/product-byKhoId")]
        public async Task<ActionResult<SanPhamFollowingKhoResponseDto[]>> GetSanPhamByKhoId([FromQuery] Guid khoId)
        {
            try
            {
                var result = await _productService.GetSanPhamByKhoIdAsync(khoId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ex.Message });
            }
        }

        [HttpPut("products/cap-nhat-so-luong")]
        public async Task<IActionResult> CapNhatSoLuong([FromBody] ChiTietKhoRequestDto[] ds)
        {
            if (ds == null || ds.Length == 0)
                return BadRequest("Không có dữ liệu để cập nhật");

            try
            {
                await _productService.CapNhatSoLuongKhoAsync(ds.ToArray());
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

