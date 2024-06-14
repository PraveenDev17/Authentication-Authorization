using Assignment.DTO.Register;
using Assignment.Models;
using Assignment.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterServices _registerService;
        private readonly IMapper _mapper;

        public RegisterController(IMapper mapper, IRegisterServices registerServices)
        {
            _mapper = mapper;
            _registerService = registerServices;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromBody] CreateRegisterDto registerDto)
        {
            var register = _mapper.Map<Register>(registerDto);
            if (await _registerService.RegisterUser(register))
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }
    }
}
