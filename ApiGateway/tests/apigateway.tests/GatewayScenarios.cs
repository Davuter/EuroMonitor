using ApiGateway.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace apigateway.tests
{
    public class GatewayScenarios : BaseScenario
    {
        [Fact]
        public async Task Get_Query_all_Subscriptions_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {

                // Expected result
                var expectedCount = 3;

                // Act
                var client = server.CreateClient();

                var response = await client
                    .GetAsync(Get.Query);

                var responseBody = await response.Content.ReadAsStringAsync();
                var subscriptions = JsonConvert.DeserializeObject<List<CatalogItem>>(responseBody);

                
                // Assert
                Assert.Equal(expectedCount, subscriptions.Count);
            }
        }

        [Fact]
        public async Task Get_Token_and_GetUserSubscriptions_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {

                // Expected result
                var expectedCount = "Bearer";

                // Act
                var client = server.CreateClient();

                var response = await client
                    .GetAsync(Get.Token("davuter1@gmail.com","Dvteda1#."));

                var responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenModel>(responseBody);

                SubscriptionsRequest subscriptionsRequest = new SubscriptionsRequest
                {
                    BuyerId = "99b20c4e-0401-48ac-90eb-cc72edd92f39"
                };

                HttpContent content = new StringContent(JsonConvert.SerializeObject(subscriptionsRequest), UTF8Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
                var userSubs = await client
               .PostAsync(Post.UserSubscriptions, content);

                var userSubsBody = await userSubs.Content.ReadAsStringAsync();
                var subscriptions = JsonConvert.DeserializeObject<UserSubscriptions>(userSubsBody);
                // Assert
                Assert.Equal(expectedCount, token.token_type);
            }
        }

        [Fact]
        public async Task Get_Token_and_AddUserSubscriptions_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {

                // Expected result
                var expectedCount = "Bearer";

                // Act
                var client = server.CreateClient();

                var response = await client
                    .GetAsync(Get.Token("davuter1@gmail.com", "Dvteda1#."));

                var responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenModel>(responseBody);

                SubscriptionAddRequest subscriptionsRequest = new SubscriptionAddRequest
                {
                    BuyerId = "99b20c4e-0401-48ac-90eb-cc72edd92f39",
                    ProductId = 5,
                    ProductName = "test"
                };

                HttpContent content = new StringContent(JsonConvert.SerializeObject(subscriptionsRequest), UTF8Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
                var userSubs = await client
               .PostAsync(Post.AddUserSubscription, content);

                var userSubsBody = await userSubs.Content.ReadAsStringAsync();
                var scription = JsonConvert.DeserializeObject<SubcriptionAddResponse>(userSubsBody);
                // Assert
                Assert.Equal(expectedCount, token.token_type);
                Assert.True(scription.Id > 0);
            }
        }

        [Fact]
        public async Task Get_Token_and_CancelUserSubscriptions_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {

                // Expected result
                var expectedCount = "Bearer";

                // Act
                var client = server.CreateClient();

                var response = await client
                    .GetAsync(Get.Token("davuter1@gmail.com", "Dvteda1#."));

                var responseBody = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenModel>(responseBody);

                SubscriptionCancelRequest subscriptionsRequest = new SubscriptionCancelRequest
                {
                    BuyerId = "99b20c4e-0401-48ac-90eb-cc72edd92f39",
                    ProductId = 5,
                    Id = 21
                };

                HttpContent content = new StringContent(JsonConvert.SerializeObject(subscriptionsRequest), UTF8Encoding.UTF8, "application/json");

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.access_token);
                var userSubs = await client
               .PostAsync(Post.CancelUserSubscription, content);

                var userSubsBody = await userSubs.Content.ReadAsStringAsync();
                var scription = JsonConvert.DeserializeObject<SubscriptionCancelResponse>(userSubsBody);
                // Assert
                Assert.Equal(expectedCount, token.token_type);
                Assert.Equal( 0, scription.ResultCode);
            }
        }


    }
}
