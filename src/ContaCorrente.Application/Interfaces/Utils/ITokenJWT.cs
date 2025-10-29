namespace ContaCorrente.Application.Interfaces.Utils
{
    public interface ITokenJWT
    {
        string GerarToken(string dado);
        string? BuscarUsuarioToken(string? token);
    }
}
