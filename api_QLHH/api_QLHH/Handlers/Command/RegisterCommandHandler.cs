using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;

namespace api_QLHH.Handlers.Command
{
    public class RegisterCommandHandler
    {
        private readonly IPersonService _personService;

        public RegisterCommandHandler(IPersonService personService)
        {
            _personService = personService;
        }
        public async Task<UserRequestDto> Handle(AccountResponseDto dto)
        {
            var newUser = await _personService.RegisterAsync(dto);
            return newUser;
        }
    }
}
