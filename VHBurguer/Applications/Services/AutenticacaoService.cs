using VHBurguer.Applications.Autenticacao;
using VHBurguer.DTOs.AutenticacaoDto;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        private static bool VerficarSenha(string senhaDigitada, byte[] senhaHashBanco)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));

            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDto Login (LoginDto loginDto)
        {
            var usuario = _repository.ObterPorEmail(loginDto.Email);
            if (usuario == null)
            {
                throw new DomainException("Email ou senha inválidos.");
            }

            if(VerficarSenha(LoginDto.Senha, usuario.Senha))
            {
                throw new DomainException("Email ou senha inválidos.");
            }
            
            var token = _tokenJwt.GerarToken(usuario);

            TokenDto novoToken = new TokenDto
            {
                Token = token
            };  

            return novoToken;   

        }
}
