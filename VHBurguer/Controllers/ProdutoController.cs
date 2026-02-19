using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        // ===============================
        // 🔐 Obter ID do usuário logado
        // ===============================
        private int ObterUsuarioIdLogado()
        {
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idTexto))
                throw new DomainException("Usuário não autenticado");

            return int.Parse(idTexto);
        }

        // ===============================
        // 📦 Listar produtos
        // ===============================
        [HttpGet]
        public ActionResult<List<LerProdutoDto>> Listar()
        {
            var produtos = _service.Listar();
            return Ok(produtos);
        }

        // ===============================
        // 🔎 Buscar produto por ID
        // ===============================
        [HttpGet("{id}")]
        public ActionResult<LerProdutoDto> ObterPorId(int id)
        {
            var produto = _service.ObterPorId(id);

            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        // ===============================
        // 🖼️ Buscar imagem do produto
        // ===============================
        [HttpGet("{id}/imagem")]
        public ActionResult ObterImagem(int id)
        {
            try
            {
                var imagem = _service.ObterImagem(id);

                if (imagem == null)
                    return NotFound();

                return File(imagem, "image/jpeg");
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
