using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.DTOs.AutenticacaoDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutentacacaoController : ControllerBase
    {
        private readonly AutentacacaoController _service;

        public AutenticacaoController(AutentacacaoController service)
        {
            _service = service;
        }

        [HttpPost("login")]

        public ActionResult<TokenDto> Login (LoginDto loginDto)
        {
            try
            {
                var token = _service.Login(loginDto);
                return Ok(token);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
