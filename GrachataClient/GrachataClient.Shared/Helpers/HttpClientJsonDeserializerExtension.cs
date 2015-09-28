using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace GrachataClient.Helpers
{
    public static class HttpClientJsonDeserializerExtension
    {
        public static async Task<T> GetDecodeAsync<T>(this HttpClient client, string address, params object[] args)
        {
            int anum = 0;
            var query = string.Join("&", from a in args
                from val in a.NameValueFromAny(anum ++)
                select val);

            var response = await client.GetAsync(string.Join("?", address, query));
            if (response.IsSuccessStatusCode)
            {
                return await DeserializeAsync<T>(response);
            }

            throw new HttpRequestException(await response.Content.ReadAsStringAsync());
        }

        public static async Task<T> PostDecodeAsync<T>(this HttpClient client, string address, object graph)
        {
            var json = new DataContractJsonSerializer(graph.GetType());

            using (var outStream = new MemoryStream())
            {
                json.WriteObject(outStream, graph);
                outStream.Position = 0;
                return await PostDecodeAsync<T>(client, address, new StreamContent(outStream));
            }
        }

        public static async Task<T> PostDecodeAsync<T>(this HttpClient client, string address, HttpContent content)
        {
            var response = await client.PostAsync(address, content);
            if (response.IsSuccessStatusCode)
            {
                return await DeserializeAsync<T>(response);
            }

            throw new HttpRequestException(await response.Content.ReadAsStringAsync());
        }

        public static async Task<T> DeserializeAsync<T>(this HttpResponseMessage response)
        {
            var json = new DataContractJsonSerializer(typeof(T));
            var resultContent = await response.Content.ReadAsByteArrayAsync();
            using (var innerStream = new MemoryStream(resultContent))
            {
                return (T)json.ReadObject(innerStream);
            }
        }

        public static string[] NameValueFromAny(this object graph, int? argNum = null, string argName = null)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));
            var objectTypeInfo = graph.GetType().GetTypeInfo();
            if (objectTypeInfo.IsClass)
                return NameValueFromObject(graph);
            var name = argName ?? "p";
            if (argNum > 0)
                name = string.Concat(name, argNum);

            return new string[] {string.Join("=", name, graph)};
        }

        public static string[] NameValueFromObject<T>(this T graph) where T : class
        {
            var typeInfo = typeof(T).GetTypeInfo();
            var isDataContract = typeInfo.IsDefined(typeof(DataContractAttribute));


            return (from p in typeof(T).GetRuntimeProperties()
                where p.GetValue(graph) != null
                let name = isDataContract &&

                           p.IsDefined(typeof(DataMemberAttribute))
                    ? ((DataMemberAttribute)p.GetCustomAttribute(typeof(DataMemberAttribute))).Name
                    : p.Name
                select string.Join("=", name, WebUtility.UrlEncode(p.GetValue(graph).ToString()))).ToArray();

        }
    }
}