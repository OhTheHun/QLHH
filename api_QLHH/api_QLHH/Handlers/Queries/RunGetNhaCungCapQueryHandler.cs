using api_QLHH.Services;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Handlers.Queries
{
    public class RunGetNhaCungCapQueryHandler
    {
        private readonly IPersonService _personService;
        public RunGetNhaCungCapQueryHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<NhaCungCap[]> Handle()
        {
            return await _personService.GetNhaCungCapListAsync();
        }
    }

}
