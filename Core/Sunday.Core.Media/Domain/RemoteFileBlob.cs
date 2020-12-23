using System;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sunday.Core.Media.Domain
{
    public class RemoteFileBlob : ApplicationBlob
    {
        private readonly string _endpoint;
        private readonly string _apiKey;
        public RemoteFileBlob(string identifier, string uploadEndPoint, string apiKey) : base(identifier)
        {
            _endpoint = uploadEndPoint;
            _apiKey = apiKey;
        }

        public override void Write(Stream stream)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", _apiKey);
            using var formData = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(stream);
            formData.Add(fileStreamContent, "File", Identifier);
            var response = client.PostAsync(_endpoint, formData).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Failed to upload file to media server. Return code {response.StatusCode}");
            }
            var data = response.Content.ReadAsStringAsync().Result;
            var identifier = JsonConvert.DeserializeObject<JObject>(data)["id"]!.ToString();
            Identifier = identifier;
        }
    }
}
