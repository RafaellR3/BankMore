using Conta.Domain.Commom;
using Conta.Domain.Movimentos;

namespace Conta.Application.Movimentos.Commands.Handlers
{
    public class EstornarMovtoHandler
    {
        private readonly IRepMovimento _repMovimento;
        public EstornarMovtoHandler(IRepMovimento repMovimento)
        {
            _repMovimento = repMovimento;
        }

        public async Task<Resultado> Handle(EstornarMovtoCommand request, CancellationToken cancellationToken)
        {
            var movimento = await _repMovimento.ObeterPorIdAsync(request.idMovto);

            if (movimento == null)
                return Resultado.Falha(" INVALID_DOCUMENT", "Movimento não localizado.");

            var estorno = new Movimento
            {
                IdContaCorrente = movimento.IdContaCorrente,
                DataMovimentacao = DateTime.Now,
                TipoMovimento = movimento.TipoMovimento == EnumTipoMovto.Credito ? EnumTipoMovto.Debito : EnumTipoMovto.Credito,
                Valor = movimento.Valor
            };
            await _repMovimento.AdicionarAsync(estorno);

            return Resultado.Ok(new
            {
                estorno.Id,
                estorno.TipoMovimento,
                estorno.Valor,
                estorno.DataMovimentacao
            });
        }
    }
}
