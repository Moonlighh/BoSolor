using Newtonsoft.Json.Linq;

namespace BODEGA_SOLORZANO.Service
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6InJleWVzYW50aWNvbmEyNUBnbWFpbC5jb20ifQ.QqJNQObIG89AMQUMLmE5f_5gXrjrwEvZgteVi1HFdB0";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> ConsultarAPIsPeru(string campo)
        {
            if (string.IsNullOrWhiteSpace(campo))
            {
                throw new ArgumentException("El campo no puede estar vacío.", nameof(campo));
            }

            try
            {
                string url;
                if (campo.Length == 8)
                {
                    url = $"https://dniruc.apisperu.com/api/v1/dni/{campo}?token={_apiToken}";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    if (responseBody != null)
                    {
                        JObject jsonObj = JObject.Parse(responseBody);
                        string nombres = jsonObj["nombres"].ToString();
                        string apellidoPaterno = jsonObj["apellidoPaterno"].ToString();
                        string apellidoMaterno = jsonObj["apellidoMaterno"].ToString();
                        string nombreCompleto = $"{nombres} {apellidoPaterno} {apellidoMaterno}";
                        return nombreCompleto;
                    }
                }
                else
                {
                    url = $"https://dniruc.apisperu.com/api/v1/ruc/{campo}?token={_apiToken}";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (responseBody != null)
                    {
                        JObject jsonObj = JObject.Parse(responseBody);
                        string razonSocial = jsonObj["razonSocial"].ToString();
                        return razonSocial;
                    }
                }
                return null;
            }
            catch (HttpRequestException e)
            {
                throw new ApplicationException($"Error al obtener los datos del DNI: {e.Message}");
            }
        }
    }
}

