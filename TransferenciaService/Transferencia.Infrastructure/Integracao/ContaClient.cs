using System.Net.Http.Json;
using Transferencia.Infrastructure.Integracao.Dto;

namespace Transferencia.Infrastructure.Integracao
{

    public class ContaClient : IContaClient
    {
        private readonly HttpClient _http;

        public ContaClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ContaDto?> ObterContaAsync(int numeroConta)
        {
            var response = await _http.GetAsync($"/api/conta/{numeroConta}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ContaDto>();
        }

        public async Task<decimal?> ObterSaldoAsync(int numeroConta)
        {
            var response = await _http.GetAsync($"/api/conta/{numeroConta}/saldo");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<SaldoDto>();
            return result?.SaldoAtual;
        }
    }
}
