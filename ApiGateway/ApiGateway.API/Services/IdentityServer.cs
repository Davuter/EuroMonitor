using ApiGateway.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ApiGateway.API.Services
{
    public class IdentityServer : IIdentityService
    {
        private readonly HttpClient _httpClient;
        public IdentityServer(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private enum ActionName
        {
            GET_Token = 1, // 

        }
        private string GetStringUrl(ActionName actionName, string jsonParameterStr = "")
        {
            string mActionName = "";

            switch (actionName)
            {
                case ActionName.GET_Token:
                    mActionName = "/connect/token";
                    break;

            }

            return mActionName;

        }

        public async Task<TokenModel> GetToken(string username, string password)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("client_id", "apigatewayapi");
            dict.Add("client_secret", "secret");
            dict.Add("grant_type", "client_credentials");
            dict.Add("username", username);
            dict.Add("password", password);
            dict.Add("scope", "apigateway");
           

            var response = await _httpClient.PostAsync(GetStringUrl(ActionName.GET_Token), new FormUrlEncodedContent(dict));


            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            TokenModel res = JsonConvert.DeserializeObject<TokenModel>(result);

            return res;
        }


    }
}
