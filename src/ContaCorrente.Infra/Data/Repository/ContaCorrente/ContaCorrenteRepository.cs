using ContaCorrente.Domain.ContasCorrentes.Entities;
using ContaCorrente.Domain.ContasCorrentes.Repository;
using ContaCorrente.Infra.Data.Context;
using Dapper;

namespace ContaCorrente.Infra.Data.Repository.ContaCorrente
{
    public class ContaRepository : IContaRepository
    {
        private readonly DapperContext _context;

        public ContaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ContaCorrenteEntity conta)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                INSERT INTO 
                    contacorrente (idcontacorrente, 
                                    numero, 
                                    nome, 
                                    ativo, 
                                    senha, 
                                    salt)
                VALUES (@IdContaCorrente, 
                        @Numero, 
                        @Nome, 
                        @Ativo, 
                        @Senha, 
                        @Salt)";
            await connection.ExecuteAsync(sql, conta);
        }

        public async Task<long> ObterProximoNumeroAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                SELECT 
                    IFNULL(MAX(numero), 100000) + 1 
                FROM contacorrente";
            return await connection.ExecuteScalarAsync<long>(sql);
        }

        public bool ExisteContaPorCpf(string cpf, bool ative = false)
        {
            using var connection = _context.CreateConnection();
            string query = @"
                SELECT 
                    COUNT(1) 
                FROM contacorrente 
                WHERE 
                    idcontacorrente = @Cpf";

            if (ative) 
            {
                query += "AND ativo = 1";
            }

            var result = connection.QueryFirstOrDefault<int>(query,new { Cpf = cpf });

            return result > 0;
        }

        public async Task<ContaCorrenteEntity?> BuscarContaCorrenteAsync(string? usuario)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<ContaCorrenteEntity>(@"
                SELECT 
                    * 
                FROM contacorrente 
                WHERE   
                    idcontacorrente = @Usuario 
                    OR CAST(numero AS TEXT) = @Usuario",
                new { Usuario = usuario }
            );

            return result;
        }

        public async Task InativarContaCorrenteAsync(string id)
        {
            using var connection = _context.CreateConnection();
            const string sql = @"
                UPDATE 
                    contacorrente
                SET 
                    ativo = 0
                WHERE 
                    idcontacorrente = @id;";

            await connection.ExecuteAsync(sql, new { id });
        }

        public async Task<double?> BuscarSaldoContaCorrenteAsync(string? idContaCorrente)
        {
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<double?>(@"
                SELECT 
                    SUM(
                        CASE
                            WHEN tipomovimento = 'D' THEN
                                valor * -1
                            ELSE
                                valor				
                        END
                    ) AS saldo 
                FROM movimento 
                WHERE 
                    idcontacorrente = @IdContaCorrente",
                new { IdContaCorrente = idContaCorrente }
            );

            return result;
        }
    }
}
