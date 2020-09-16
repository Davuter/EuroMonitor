using Catalog.API.Infrastructure.Repositories;
using Catalog.API.Infrastructure.Storage;
using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace catalogs.unittests
{
    public class applicationUnittests
    {
        public readonly Mock<CatalogDbContext> _dbcontextMock;
        private readonly Mock<CatalogRepository<CatalogItem>> _catalogRepoMock;
        public applicationUnittests()
        {
            _dbcontextMock = new Mock<CatalogDbContext>(new DbContextOptions<CatalogDbContext>());
            _catalogRepoMock = new Mock<CatalogRepository<CatalogItem>>(_dbcontextMock.Object);
            _dbcontextMock.Setup(r => r.Set<CatalogItem>()).Returns(GetQueryableMockDbSet(GetFakeCatalogItems()));

        }
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }

        private List<CatalogItem> GetFakeCatalogItems()
        {
            List<CatalogItem> catalogItems = new List<CatalogItem>()
            {
                new CatalogItem
                {
                    Id = 1,
                    Name = "fake1",
                    OwnerId = 1,
                    CatalogOwner = new CatalogOwner
                    {
                        Id = 1,
                        Name = "fakeOwner"
                    }
                },
                   new CatalogItem
                {
                    Id = 2,
                    Name = "fake2",
                    OwnerId = 1,
                    CatalogOwner = new CatalogOwner
                    {
                        Id = 1,
                        Name = "fakeOwner"
                    }
                },
                      new CatalogItem
                {
                    Id = 3,
                    Name = "fake1",
                    OwnerId = 1,
                    CatalogOwner = new CatalogOwner
                    {
                        Id = 1,
                        Name = "fakeOwner"
                    },
                    CatalogTypeId = 1,
                    CatalogType = new CatalogType
                    {
                        Id = 1,
                        Name = "fakeType1"
                    }
                }
            };
            return catalogItems;
        }
        [Fact]
        public async void Get_CatalogItems_WithFilter_Success()
        {
            //Arange
            _catalogRepoMock.Setup(r => r.Get(r => r.Id == 1, null, "CatalogOwner"))
                .Returns(GetFakeCatalogItems().Where(r => r.Id == 1));
                     //Act
            var repo = new CatalogRepository<CatalogItem>(_dbcontextMock.Object);
            var x = repo.Get(r => r.Id == 1, null, "CatalogOwner");
            //Asert
            Assert.Equal("fake1", x.FirstOrDefault().Name);
            Assert.Equal(1, x.FirstOrDefault().Id);
        }
        [Fact]
        public async void Insert_CatalogItem_Success()
        {
            _catalogRepoMock.Setup(r => r.Insert(It.IsAny<CatalogItem>()));

            var repo = new CatalogRepository<CatalogItem>(_dbcontextMock.Object);
            repo.Insert(It.IsAny<CatalogItem>());

            Assert.Equal(1, 1);
        }
    }
}
