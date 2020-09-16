using ApiGateway.API.Controllers;
using ApiGateway.API.Models;
using ApiGateway.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace apigateway.unittest
{
    public class gatewayControllerTest
    {
        private readonly Mock<ICatalogServices> _catologService;
        private readonly Mock<IIdentityService> _identityService;
        private readonly Mock<ISubscriptionService> _subscriptionService;
        public gatewayControllerTest()
        {
            _catologService = new Mock<ICatalogServices>();
            _identityService = new Mock<IIdentityService>();
            _subscriptionService = new Mock<ISubscriptionService>();
        }

        [Fact]
        public async void Get_All_CatalogItems_Success()
        {

            _catologService.Setup(r => r.GetCatalogItems()).Returns(Task.FromResult(GetFakeCatalogItems()));
          

            var controller = new GatewayController(_catologService.Object,_subscriptionService.Object,
                _identityService.Object);

            var x = await controller.GetCatalogItems();
           
            Assert.True(x.Count> 0);
           

        }

         
        private List<CatalogItem> GetFakeCatalogItems()
        {
            List<CatalogItem> catalogItems = new List<CatalogItem>
            {
                new CatalogItem
                {
                    Id = 1,
                    Name = "Fake Product 1",
                    Price = 10
                },
                new CatalogItem
                {
                      Id = 2,
                    Name = "Fake Product 2",
                    Price = 11
                }
            };

            return catalogItems;
        }
    }
}
