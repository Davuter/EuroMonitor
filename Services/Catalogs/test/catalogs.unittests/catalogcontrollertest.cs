using Catalog.API.Controllers;
using Catalog.API.Infrastructure.Repositories;
using Catalog.API.Infrastructure.UnitofWorks;
using Catalog.API.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace catalogs.unittests
{
    public class catalogcontrollertest
    {
        private readonly Mock<IUnitofWork> _unitofWork;
        public catalogcontrollertest()
        {
            _unitofWork = new Mock<IUnitofWork>();

        }

        [Fact]
        public async void Get_All_CatalogItems_Success()
        {
            var mockCatalogItemRepository = new Mock<ICatalogRepository<CatalogItem>>();
            mockCatalogItemRepository.Setup(r => r.Get(null, null, "CatagoryOwner,CatagoryItem")).Returns(GetFakeCatalogItems());
            _unitofWork.Setup(r => r.CatalogItemRepository).Returns(mockCatalogItemRepository.Object);

            var controller = new CatalogController(_unitofWork.Object);

            var x = await controller.GetCatalogItemsAsync();
            var okResult = x as OkObjectResult;
            Assert.IsType<OkObjectResult>(x);
            Assert.Equal(200, okResult.StatusCode);

        }

        [Fact]
        public async void Get_All_CatalogItemById_Success()
        {
            var mockCatalogItemRepository = new Mock<ICatalogRepository<CatalogItem>>();
            mockCatalogItemRepository.Setup(r => r.GetByID(It.IsAny<int>())).Returns(GetFakeCatalogItems().GetEnumerator().Current);
            _unitofWork.Setup(r => r.CatalogItemRepository).Returns(mockCatalogItemRepository.Object);

            var controller = new CatalogController(_unitofWork.Object);

            var x = await controller.GetCatalogItemAsync(It.IsAny<int>());
            var okResult = x as OkObjectResult;
            Assert.IsType<OkObjectResult>(x);
            Assert.Equal(200, okResult.StatusCode);

        }

        private IEnumerable<CatalogItem> GetFakeCatalogItems()
        {
            List<CatalogItem> catalogItems = new List<CatalogItem>()
            {
                new CatalogItem
                {
                    Id = 1,
                    Name = "fake1",
                    Price = 10,
                    Description = ""
                },
                      new CatalogItem
                {
                    Id = 2,
                    Name = "fake2",
                    Price = 10,
                    Description = ""
                },
            };
            return catalogItems;
        }
    }
}
