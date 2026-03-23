using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeknoBideTPV.DTOak;

namespace TeknoBideTPV.Zerbitzuak
{
    public class ApiZerbitzua
    {
        public static readonly string BASE_URL = "http://192.168.1.112:5000/";
        //public static readonly string BASE_URL = "http://localhost:5000/";

        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri(BASE_URL)
        };

        public async Task<LoginErantzunaDto?> LoginAsync(int langileKodea, string pasahitza)
        {
            var request = new { Langile_kodea = langileKodea, Pasahitza = pasahitza };
            var response = await _httpClient.PostAsJsonAsync("api/login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginErantzunaDto>();
        }

        public async Task<List<ProduktuaDto>> LortuProduktuakAsync()
        {
            var produktuak = await _httpClient.GetFromJsonAsync<List<ProduktuaDto>>("api/produktuak");
            return produktuak ?? new List<ProduktuaDto>();
        }

        public async Task<EskariaSortuErantzunaDto?> SortuEskariaAsync(EskariaSortuDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/eskariak", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<EskariaSortuErantzunaDto>();
        }

        public async Task<List<EskariaDto>> LortuEskariakAsync()
        {
            var eskariak = await _httpClient.GetFromJsonAsync<List<EskariaDto>>("api/eskariak");
            return eskariak ?? new List<EskariaDto>();
        }

        public async Task<List<ErreserbaDto>> ErreserbakLortuAsync()
        {
            var erreserbak = await _httpClient.GetFromJsonAsync<List<ErreserbaDto>>("api/erreserbak");
            return erreserbak ?? new List<ErreserbaDto>();
        }

        public async Task<bool> SortuErreserbaAsync(ErreserbaSortuDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/erreserbak", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> OrdainduErreserbaAsync(ErreserbaOrdainduDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/erreserbak/ordaindu", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MahaiaDto>> MahaiakLortuAsync()
        {
            var mahaiak = await _httpClient.GetFromJsonAsync<List<MahaiaDto>>("api/mahaiak");
            return mahaiak ?? new List<MahaiaDto>();
        }
        public async Task<bool> SortuMahaiaAsync(MahaiaSortuDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/mahaiak", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> AktualizatuMahaiaAsync(MahaiaDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/mahaiak/{dto.Id}", dto);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> EzabatuMahaiaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/mahaiak/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EzabatuErreserbaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/erreserbak/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateErreserbaAsync(int id, ErreserbaSortuDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/erreserbak/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EguneratuEskariaAsync(int id, EskariaSortuDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/eskariak/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EzabatuEskariaAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/eskariak/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
