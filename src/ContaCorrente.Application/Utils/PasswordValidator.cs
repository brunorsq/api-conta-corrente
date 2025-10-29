using ContaCorrente.Application.Interfaces.Utils;
using ContaCorrente.Domain._Shared;

namespace ContaCorrente.Application.Utils
{
    public class PasswordValidator : IPasswordValidator
    {
        public bool ValidarSenha(string senhaRequest, string senhaConta, string saltConta)
        {
            var senhaHash = new GerarHash(senhaRequest, saltConta).Hash;

            if (senhaHash != senhaConta)
                return false;
            return true;
        }
    }
}
