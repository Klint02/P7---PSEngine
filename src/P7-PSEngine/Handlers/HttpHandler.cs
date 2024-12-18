using P7_PSEngine.DTO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace P7_PSEngine.Handlers;

public static class HttpHandler
{
    public static async Task<DataErrorDTO> FormUrlEncodedAsyncPost(FormUrlEncodedContent content, string url)
    {
        using (HttpClient client = new HttpClient())
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };

            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new DataErrorDTO { Data = await response.Content.ReadAsStringAsync(), Error = "" };
            }
            else
            {
                return new DataErrorDTO { Data = await response.Content.ReadAsStringAsync(), Error = response.StatusCode.ToString() };
            }
        }
    }

    public static async Task<DataErrorDTO> JSONAsyncPost(object file_request_body, string url, string? token)
    {
        string content = JsonSerializer.Serialize(file_request_body);


        using (HttpClient client = new HttpClient())
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post,
            };

            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new DataErrorDTO { Data = await response.Content.ReadAsStringAsync(), Error = "" };
            }
            else
            {
                return new DataErrorDTO { Data = await response.Content.ReadAsStringAsync(), Error = response.StatusCode.ToString() };
            }
        }
    }
}