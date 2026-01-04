using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;

namespace api_QLHH.Handlers.Queries
{
    public class LoginUserQueryHandler
    {
        private readonly IPersonService _personService;
        public LoginUserQueryHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<UserResponseDto> Handle(AccountResponseDto acc)
        {
            return await _personService.LoginAsync(acc);
        }
    }
}
