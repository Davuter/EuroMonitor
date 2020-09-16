using ApiGateway.API.Config;
using ApiGateway.API.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.API.Services
{
    public class CatalogServices : ICatalogServices
    {

        private readonly HttpClient _httpClient;
        public CatalogServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private enum ActionName
        {
            GET_Catalog_Items = 1, // 

        }
        private string GetStringUrl(ActionName actionName, string jsonParameterStr = "")
        {
            string mActionName = "";

            switch (actionName)
            {
                case ActionName.GET_Catalog_Items:
                    mActionName = "api/Catalog/Items";
                    break;

            }

            return mActionName;

        }
        public async Task<List<CatalogItem>> GetCatalogItems()
        {
            try
            {
                var response = await _httpClient.GetAsync(GetStringUrl(ActionName.GET_Catalog_Items));

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;

                List<CatalogItem> res = JsonConvert.DeserializeObject<List<CatalogItem>>(result);

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
