using api_QLHH.Core.DTOs.Requests;
using api_QLHH.Core.DTOs.Responses;
using api_QLHH.Services.Interface;
using System;

namespace api_QLHH.Handlers.Commands
{
    public class UpdateUserCommandHandler
    {
        private readonly IPersonService _personService;

        public UpdateUserCommandHandler(IPersonService personService)
        {
            _personService = personService;
        }
        public async Task<UserRequestDto> Handle(Guid userId, UserRequestDto dto)
        {
            var updatedUser = await _personService.UpdateAsync(userId, dto);
            return updatedUser;
        }
    }
}
