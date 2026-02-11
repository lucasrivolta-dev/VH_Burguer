using VHBurguer.DTOs;
using VHBurguer.Interfaces;
using VHBurguer.Domains;
using VHBurguer.Exceptions;
using System.Security.Cryptography;
using System.Text;
namespace VHBurguer.Applications.Services
{
    // Service Concentra o "como fazer"
    public class UsuarioService
    {
        // repository e o canal para acessar os dados 
        private readonly IUsuarioRepository _repository;
        // injeção de dependência
        //implementamos o repositorio e o service sp  depende da interface do repositorio

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        private static LerUsuarioDto LerDto(Usuario usuario)
        {
            LerUsuarioDto usuarioDto = new LerUsuarioDto
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true
            };
            return usuarioDto;
        }

        public List<LerUsuarioDto> Listar()
        {
            List<Usuario> usuarios = _repository.Listar();
            List<LerUsuarioDto> usuariosDto = usuarios.Select(usuarioBanco => LerDto(usuarioBanco)).ToList();
            return usuariosDto;
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email inválido.");
            }

        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                throw new DomainException("Senha é obrigatoria.");
            }

            using var sha256 = SHA256.Create(); // Gera um hash da senha usando SHA256 e devolve em byte
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerUsuarioDto ObterPorId(int id)
        {
            Usuario? usuario = _repository.ObterPorId(id);

            if (usuario == null)
            {
                throw new DomainException("Usuario não encontrado.");
            }

            return LerDto(usuario);
        }


    }
}
