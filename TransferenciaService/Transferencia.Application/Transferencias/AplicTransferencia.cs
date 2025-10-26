using System.Net.Http.Headers;
using System.Net.Http.Json;
using Transferencia.Domain.Transferencias;
using Microsoft.Extensions.Configuration;
using Conta.Infrastructure.Resultados;

namespace Transferencia.Application.Transferencias
{
    public class AplicTransferencia : IAplicTransferencia
    {
        private readonly IRepTransferencia _repTransferencia;
        private readonly HttpClient _http;
        private readonly string _baseUrl;

        public AplicTransferencia(IRepTransferencia repTransferencia, 
                                  HttpClient http,
                                  IConfiguration config)
        {
            _repTransferencia = repTransferencia;
            _http = http;
            _baseUrl = config["Services:ContaService"]
                       ?? throw new ArgumentNullException("Services:ContaService", "URL do serviço de Conta não configurada");
        }

        public async Task<Resultado> EfetuarAsync(string origem, string destino, decimal valor, string token)
        {
            
            var resultadoD = await ChamarContaApiAsync($"{_baseUrl}/api/Movimento",
                                      new { NumeroConta = origem, Tipo = 1, Valor = valor }, token,
                                      "Falha ao debitar conta origem");

            if (!resultadoD.Sucesso) 
                return resultadoD;

            var resultadoC = await ChamarContaApiAsync($"{_baseUrl}/api/Movimento",
                                      new { NumeroConta = destino, Tipo = 0, Valor = valor }, token,
                                      "Falha ao creditar conta destino");

            if (!resultadoC.Sucesso)
                return resultadoC;

            var transferencia = new Transfer(origem, destino, valor);
            await _repTransferencia.SalvarAsync(transferencia);

            return Resultado.Ok();
        }

        private async Task<Resultado> ChamarContaApiAsync(string url, object payload, string token, string mensagemErro)
        {
            var idempotencyKey = Guid.NewGuid().ToString();

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = JsonContent.Create(payload)
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("Idempotency-Key", idempotencyKey);

            var response = await _http.SendAsync(request);

            var resultado = await response.Content.ReadFromJsonAsync<Resultado>();

            if (!response.IsSuccessStatusCode || resultado == null || !resultado.Sucesso)
            {
                var detalhe = resultado?.Mensagem ?? await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"{mensagemErro}. Detalhes: {detalhe}");
            }

            return resultado;
        }
    }
}