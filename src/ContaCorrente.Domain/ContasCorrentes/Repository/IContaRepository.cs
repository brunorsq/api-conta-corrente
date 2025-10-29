using ContaCorrente.Domain.ContasCorrentes.Entities;

namespace ContaCorrente.Domain.ContasCorrentes.Repository
{
    public interface IContaRepository
    {
        bool ExisteContaPorCpf(string cpf, bool ative = false);
        Task CreateAsync(ContaCorrenteEntity conta);
        Task<long> ObterProximoNumeroAsync();
        Task<ContaCorrenteEntity?> BuscarContaCorrenteAsync(string usuario);
        Task InativarContaCorrenteAsync(string id);
        Task<double?> BuscarSaldoContaCorrenteAsync(string? idContaCorrente);
    }
}
