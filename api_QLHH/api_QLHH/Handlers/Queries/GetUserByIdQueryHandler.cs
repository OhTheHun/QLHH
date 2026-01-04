using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;
using api_QLHH.SqlData.Models;

namespace api_QLHH.Handlers.Queries
{
    public class GetUserByIdQueryHandler
    {
        private readonly IPersonService _personService;
        public GetUserByIdQueryHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<UserResponseDto> Handle(Guid userId)
        {
            return await _personService.GetByIdAsync(userId);
        }
    }

}
