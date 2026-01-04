using api_QLHH.Services;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Handlers.Queries
{
    public class RunGetKhachHangQueryHandler
    {
        private readonly IPersonService _personService;
        public RunGetKhachHangQueryHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<KhachHang[]> Handle()
        {
            return await _personService.GetKhachHangListAsync();
        }
    }

}
