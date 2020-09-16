using Newtonsoft.Json;
using Subscription.Application.Subscriptions.Command.Add;
using Subscription.Application.Subscriptions.Command.Cancel;
using Subscription.Application.Subscriptions.Query;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Subscription.FunctionalTest
{
    public class SubscriptionScenarios : BaseScenario
    {
        [Fact]
        public async Task Post_Query_all_Subscriptions_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var userId = "99b20c4e-0401-48ac-90eb-cc72edd92f39";
                var req = new SubscriptionQueryIn()
                {
                    BuyerId = userId
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(req), UTF8Encoding.UTF8, "application/json");

                // Expected result
                var expectedCount = 6;

                // Act
                var client =  server.CreateClient();
             
                var response = await client
                    .PostAsync(Post.Query, content);

                var responseBody = await response.Content.ReadAsStringAsync();
                var subscriptions = JsonConvert.DeserializeObject<SubscriptionQueryOut>(responseBody);

                // Assert
                Assert.Equal(expectedCount, subscriptions.Subscriptions.Count);
            }
        }

        [Fact]
        public async Task Post_Add_New_Subscription_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var userId = "99b20c4e-0401-48ac-90eb-cc72edd92f39";
                var req = new SubscriptionAddIn()
                {
                    BuyerId = userId,
                    ProductId = 1,
                    ProductName ="Func test fake",
                    SubscriptionDate = DateTime.Now
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(req), UTF8Encoding.UTF8, "application/json");

        
                // Act
                var client = server.CreateClient();

                var response = await client
                    .PostAsync(Post.Add, content);

                var responseBody = await response.Content.ReadAsStringAsync();
                var subscription = JsonConvert.DeserializeObject<SubcriptionAddOut>(responseBody);

                // Assert
                Assert.True(subscription.Id > 0);
            }
        }
        [Fact]
        public async Task Post_Add_New_Subscription_and_AlreadyExist_request_badrequest()
        {
            using (var server = CreateServer())
            {
                var userId = "99b20c4e-0401-48ac-90eb-cc72edd92f39";
                var req = new SubscriptionAddIn()
                {
                    BuyerId = userId,
                    ProductId = 3
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(req), UTF8Encoding.UTF8, "application/json");


                // Act
                var client = server.CreateClient();

                var response = await client
                    .PostAsync(Post.Add, content);

                var responseBody = await response.Content.ReadAsStringAsync();
                var subscription = JsonConvert.DeserializeObject<SubcriptionAddOut>(responseBody);

                // Assert
                Assert.Equal(subscription.Id , 0);
            }
        }

        [Fact]
        public async Task Post_Cancel_Subscription_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var userId = "99b20c4e-0401-48ac-90eb-cc72edd92f39";
                var req = new SubscriptionCancelIn()
                {
                    BuyerId = userId,
                    Id = 13,
                    ProductId = 1
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(req), UTF8Encoding.UTF8, "application/json");


                // Act
                var client = server.CreateClient();

                var response = await client
                    .PostAsync(Post.Cancel, content);

                var responseBody = await response.Content.ReadAsStringAsync();
                var subscription = JsonConvert.DeserializeObject<SubscriptionCancelOut>(responseBody);

                // Assert
                Assert.Equal(0, subscription.ResultCode);
            }
        }

        [Fact]
        public async Task Post_Add_Subscription_and_Query_Cancel_Query_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var userId = "99b20c4e-0401-48ac-90eb-cc72edd92f39";
                var req = new SubscriptionAddIn()
                {
                    BuyerId = userId,                 
                    ProductId = 4,
                    ProductName = "Fake Product",
                    SubscriptionDate = DateTime.Now
                };
                HttpContent content = new StringContent(JsonConvert.SerializeObject(req), UTF8Encoding.UTF8, "application/json");


                // Act
                var client = server.CreateClient();

                var addresponse = await client
                    .PostAsync(Post.Add, content);

                var addresponseBody = await addresponse.Content.ReadAsStringAsync();
                var addsubscription = JsonConvert.DeserializeObject<SubcriptionAddOut>(addresponseBody);

                var queryreq = new SubscriptionQueryIn()
                {
                    BuyerId = userId,
                    ProductId = 4
                };
                HttpContent querycontent = new StringContent(JsonConvert.SerializeObject(queryreq), UTF8Encoding.UTF8, "application/json");

                var queryresponse = await client
                    .PostAsync(Post.Query, querycontent);

                var queryresponseBody = await queryresponse.Content.ReadAsStringAsync();
                var querysubscription = JsonConvert.DeserializeObject<SubscriptionQueryOut>(queryresponseBody);

                var cancelreq = new SubscriptionQueryIn()
                {
                    BuyerId = userId,
                    ProductId = 4,
                    Id = addsubscription.Id
                };
                HttpContent cancelcontent = new StringContent(JsonConvert.SerializeObject(cancelreq), UTF8Encoding.UTF8, "application/json");

                var cancelresponse = await client
                    .PostAsync(Post.Cancel, cancelcontent);

                var cancelresponseBody = await cancelresponse.Content.ReadAsStringAsync();
                var cancelsubscription = JsonConvert.DeserializeObject<SubscriptionCancelOut>(cancelresponseBody);

                // Assert
                Assert.True(addsubscription.Id> 0);
                Assert.True(querysubscription.Subscriptions.Count > 0);
                Assert.Equal(0,cancelsubscription.ResultCode);
            }
        }
    }
}
