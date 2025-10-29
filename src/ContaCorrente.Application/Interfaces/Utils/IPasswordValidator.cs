namespace ContaCorrente.Application.Interfaces.Utils
{
    public interface IPasswordValidator
    {
        bool ValidarSenha(string senhaRequest, string senhaConta, string saltConta);
    }
}
