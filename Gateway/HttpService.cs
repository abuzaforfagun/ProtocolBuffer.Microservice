using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ProtoBuf;

namespace Gateway
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(string url);
    }

    public class HttpService : IHttpService
    {
        public async Task<T> GetAsync<T>(string url)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));
            var response = await client.SendAsync(request);
            return Serializer.Deserialize<T>(await response.Content.ReadAsStreamAsync());
        }
    }
}
