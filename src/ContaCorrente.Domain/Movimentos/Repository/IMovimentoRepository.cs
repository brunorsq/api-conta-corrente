using ContaCorrente.Domain.Movimentos.Entities;

namespace ContaCorrente.Domain.Movimentos.Repository
{
    public interface IMovimentoRepository
    {
        public Task CreateAsync(MovimentoEntity movimento);
    }
}
