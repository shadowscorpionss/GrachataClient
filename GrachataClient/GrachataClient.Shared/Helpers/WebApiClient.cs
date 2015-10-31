using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GrachataClient.Models;
using System.Net;

namespace GrachataClient.Helpers
{



    public class WebApiClient
    {
        public string ApiBaseAddress { get; set; } = "ins/";
        public string BaseAddress { get; set; } = "http://grachata.ru/";
        public string TokenAddress { get; set; } = "Token";

        public TokenModel Token { get; set; }


        public async Task<TokenModel> LoginAsync(string userName, string password)
        {
            var loginModel = new LoginModel()
            {
                UserName = userName,
                Password = password
            };
            var grantType = $"{ LoginModel.GrantTypeString }={ loginModel.GrantType}";
            var user = $"{LoginModel.UserNameString}={WebUtility.UrlEncode(loginModel.UserName)}";
            var pass = $"{LoginModel.PasswordString}={WebUtility.UrlEncode(loginModel.Password)}";
            var callString = string.Join("&", grantType, user, pass);
            var loginContent = new StringContent(callString);

            return Token = await CallApiJsonClient("application/x-www-form-urlencoded", async client =>
            {
                var res = await client.PostDecodeAsync<TokenModel>(ApiBaseAddress + TokenAddress, loginContent);
                client.Dispose();
                return res;
            });
        }

        public async Task<T> PostAuthorizedAsync<T>(string address, object graph)
        {
            return await CallApiAuthorizedJson(async client => await client.PostDecodeAsync<T>(address, graph));
        }

        public async Task<T> GetAuthorizedAsync<T>(string address, params object[] args)
        {
            return await CallApiAuthorizedJson(async client => await client.GetDecodeAsync<T>(address, args));
        }

        T CallApiAuthorizedJson<T>(Func<HttpClient, T> clientCallBack)
        {
            if (Token == null)
                return default(T);

            return CallApiJsonClient(client =>
            {
                client.DefaultRequestHeaders.Add("Authorization", string.Join(" ", Token.TokenType, Token.AccessToken));
                return clientCallBack(client);
            });
        }

        T CallApiJsonClient<T>(string contentType, Func<HttpClient, T> clientCallBack)
        {
            return CallApiJsonClient(client =>
            {
                client.DefaultRequestHeaders.Add("ContentType", contentType);
                return clientCallBack(client);
            });
        }

        T CallApiJsonClient<T>(Func<HttpClient, T> clientCallBack)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(BaseAddress) };


            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return clientCallBack(httpClient);
        }

    }
}
