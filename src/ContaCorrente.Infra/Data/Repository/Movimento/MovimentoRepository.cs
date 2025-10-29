using ContaCorrente.Domain.Movimentos.Entities;
using ContaCorrente.Domain.Movimentos.Repository;
using ContaCorrente.Infra.Data.Context;
using Dapper;

namespace ContaCorrente.Infra.Data.Repository.ContaCorrente
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly DapperContext _context;

        public MovimentoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(MovimentoEntity movimento)
        {
            using var connection = _context.CreateConnection();

            const string sql = @"
                INSERT INTO movimento (idmovimento, 
                                        idcontacorrente, 
                                        datamovimento, 
                                        tipomovimento, 
                                        valor)
                VALUES (@IdMovimento, 
                        @IdContaCorrente, 
                        @DataMovimento, 
                        @TipoMovimento, 
                        @Valor)";

            var parametros = new
            {
                movimento.IdMovimento,
                movimento.IdContaCorrente,
                movimento.DataMovimento,
                TipoMovimento = ((char)movimento.TipoMovimento.Value).ToString(), // 👈 converte para "C" ou "D"
                movimento.Valor
            };

            await connection.ExecuteAsync(sql, parametros);
        }
    }
}
