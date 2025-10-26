using Conta.Domain.Contas;
using Conta.Domain.Dto;
using Dapper;

namespace Conta.Infrastructure
{
    public class RepConta: IRepConta
    {
        private readonly DapperContext _context;

        public RepConta(DapperContext context)
        {
            _context = context;
        }

        public async Task<ContaCorrente?> ObterLoginAsync(LoginDto dto)
        {
            using var connection = _context.CreateConnection();
            string sql;
            switch (dto.TipoLogin)
            {
                case EnumTipoLogin.Cpf :

                    sql = "SELECT Id, Cpf, Senha, Salt, Numero FROM ContaCorrente WHERE Cpf = @cpf";
                    return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Cpf = dto.Login });
                case EnumTipoLogin.NumeroConta :

                    sql = "SELECT Id, Cpf, Senha, Salt, Numero FROM ContaCorrente WHERE numero = @numero";
                    return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Numero = dto.Login });
                default:
                    throw new Exception("Login inválido.");
            }
        }

        public async Task<int> TaskObterUltimoNumeroAsync()
        {
            using var connection = _context.CreateConnection();
            const string sql = "SELECT IFNULL(MAX(Numero), 0) FROM ContaCorrente";

            var maiorNumero = await connection.ExecuteScalarAsync<int>(sql);
            return maiorNumero;
        }

        public async Task AdicionarAsync(ContaCorrente conta)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO ContaCorrente (Id, Numero, Cpf, Senha, Salt, Ativo) VALUES (@Id, @Numero, @Cpf, @Senha, @Salt, @Ativo)";
            await connection.ExecuteAsync(sql, conta);
        }

        public async Task<ContaCorrente?> ObterPorNumeroAsync(int numero)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT Id, Numero, Cpf, Senha, Salt, Ativo FROM ContaCorrente WHERE Numero = @Numero";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { Numero = numero });
        }

        public async Task AtualizarAsync(ContaCorrente conta)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE ContaCorrente SET Ativo = @Ativo WHERE Id = @Id";
            await connection.ExecuteAsync(sql, conta);
        }
    }
}
