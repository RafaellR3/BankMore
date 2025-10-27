using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Transferencia.Domain.Resultados;
using Transferencia.Infrastructure.Integracao.Dto;
using static System.Net.WebRequestMethods;

namespace Transferencia.Infrastructure.Integracao
{

    public class ContaClient : IContaClient
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContaClient(HttpClient http, IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ContaDto?> ObterContaAsync(Guid idConta)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync($"/api/ContaCorrente/{idConta}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ContaDto>();
        }

        public async Task<decimal?> ObterSaldoAsync(Guid idConta)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _http.GetAsync($"/api/ContaCorrente/{idConta}/saldo");
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<SaldoDto>();
            return result?.SaldoAtual;
        }

        public async Task<Resultado> SalvarMovto(MovtoDto dto)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length);
            var json = JsonSerializer.Serialize(dto);

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Movimento")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("Idempotency-Key", Guid.NewGuid().ToString());

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var erroJson = await response.Content.ReadAsStringAsync();
                if (erroJson.Length>0 )
                {
                    var erro = JsonSerializer.Deserialize<ErroDto>(erroJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Resultado.Falha(erro?.type, erro?.message);
                }
                return Resultado.Falha("Falha", "Erro desconhecido");
            }

            return Resultado.Ok();
        }

        public async Task<Resultado> EstornarMovto(Guid idMovto)
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault()?.Substring("Bearer ".Length);
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.PostAsync($"/api/Movimento/{idMovto}/Estorno", null);
            if (!response.IsSuccessStatusCode)
            {
                var erroJson = await response.Content.ReadAsStringAsync();
                if (erroJson.Length > 0)
                {
                    var erro = JsonSerializer.Deserialize<ErroDto>(erroJson, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return Resultado.Falha(erro?.type, erro?.message);
                }
                return Resultado.Falha("Falha", "Erro desconhecido");

            }

            return Resultado.Ok();
        }
    }
}
