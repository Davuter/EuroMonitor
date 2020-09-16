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
    public class SubscriptionService : ISubscriptionService
    {
        private readonly HttpClient _httpClient;
        public SubscriptionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        private enum ActionName
        {
            POST_Subscription_Query = 1, // 
            POST_Subscription_Add = 2,
            POST_Subscription_Cancel =3

        }
        private string GetStringUrl(ActionName actionName, string jsonParameterStr = "")
        {
            string mActionName = "";

            switch (actionName)
            {
                case ActionName.POST_Subscription_Query:
                    mActionName = "api/Subscription/Query";
                    break;
                case ActionName.POST_Subscription_Add:
                    mActionName = "api/Subscription/Add";
                    break;
                case ActionName.POST_Subscription_Cancel:
                    mActionName = "api/Subscription/Cancel";
                    break;

            }

            return mActionName;

        }

        public async Task<UserSubscriptions> GetUserSubscriptions(SubscriptionsRequest request)
        {
            try
            {
                HttpContent strCont = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(GetStringUrl(ActionName.POST_Subscription_Query), strCont);

                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;

                UserSubscriptions res = JsonConvert.DeserializeObject<UserSubscriptions>(result);


                res.BuyerId = request.BuyerId;             

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SubcriptionAddResponse> AddUserSubscription(SubscriptionAddRequest request)
        {
            request.SubscriptionDate = DateTime.Now;
            HttpContent strCont = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(GetStringUrl(ActionName.POST_Subscription_Add), strCont);

            var result = response.Content.ReadAsStringAsync().Result;

            SubcriptionAddResponse res = JsonConvert.DeserializeObject<SubcriptionAddResponse>(result);            

            return res;
        }

        public async Task<SubscriptionCancelResponse> CancelUserSubscription(SubscriptionCancelRequest request)
        {
            HttpContent strCont = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(GetStringUrl(ActionName.POST_Subscription_Cancel), strCont);

            var result = response.Content.ReadAsStringAsync().Result;

            SubscriptionCancelResponse res = JsonConvert.DeserializeObject<SubscriptionCancelResponse>(result);

            return res;
        }
    }
}
