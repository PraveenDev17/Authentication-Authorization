using System.Security.Claims;
using Assignment.DTO.Login;
using Assignment.DTO.Register;
using Assignment.Models;
using Assignment.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginServices _loginServices;
        private readonly IMapper _mapper;

        public LoginController(ILoginServices loginServices, IMapper mapper)
        {
            _loginServices = loginServices;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<string> Login([FromBody] RequestLoginDto requestLoginDto)
        {

            var credentials = _mapper.Map<Register>(requestLoginDto);
            ActionResult result = _loginServices.LoginUser(credentials);
            if (result is JsonResult jsonResult)
            {
                var result1 = jsonResult.Value.ToString();
                if (result1 != null)
                {
                    return Ok(result1);
                }
            }

            return NotFound();

        }

        [HttpGet("info")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CreateRegisterDto>> Get()
        {
            var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);
            var nameClaim = HttpContext.User.FindFirst(ClaimTypes.Name);
            if (roleClaim != null && nameClaim != null)
            {
                var role = roleClaim.Value;
                var uname = nameClaim.Value;

                switch (role)
                {
                    case "Admin":
                        var data = await _loginServices.GetAll();
                        var showData = _mapper.Map<List<CreateRegisterDto>>(data);
                        foreach (var item in showData)
                        {
                            ActionResult decryptData = _loginServices.Decrypt(item.Credit_Card);
                            if (decryptData is JsonResult jsonResult1)
                            {
                                item.Credit_Card = jsonResult1.Value.ToString();
                            }
                        }
                        return Ok(showData);
                    case "Reader":
                        var userdata = await _loginServices.Get(uname);
                        var showUserData = _mapper.Map<CreateRegisterDto>(userdata);
                        ActionResult result = _loginServices.Decrypt(showUserData.Credit_Card);
                        if (result is JsonResult jsonResult)
                        {
                            showUserData.Credit_Card = jsonResult.Value.ToString();
                        }
                        return Ok(showUserData);
                }
            }

            return NotFound();
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateDataDto updateDataDto)
        {
            if (updateDataDto == null || id != updateDataDto.Id)
            {
                return BadRequest("Invalid data provided for update.");
            }

            var existingData = await _loginServices.GetById(id);
            if (existingData == null)
            {
                return NotFound();
            }

            var updatedData = _mapper.Map<Register>(updateDataDto);
            updatedData.Id = id;

            await _loginServices.Update(updatedData);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var userData = await _loginServices.GetById(id);
            if (userData == null)
            {
                return NotFound();
            }
            await _loginServices.Delete(userData);
            return NoContent();
        }
    }
}
