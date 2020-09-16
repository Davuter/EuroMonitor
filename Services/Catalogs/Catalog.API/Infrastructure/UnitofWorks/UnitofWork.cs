using Catalog.API.Infrastructure.Repositories;
using Catalog.API.Infrastructure.Storage;
using Catalog.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.UnitofWorks
{
    public class UnitofWork : IUnitofWork
    {
        private readonly CatalogDbContext _dbContext;
        private ICatalogRepository<CatalogItem> _catalogItemRepository;
        private ICatalogRepository<CatalogOwner> _catalogOwnerRepository;
        private ICatalogRepository<CatalogType> _catalogTypeRepository;
        public UnitofWork(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICatalogRepository<CatalogItem> CatalogItemRepository
        {
            get
            {
                return _catalogItemRepository = _catalogItemRepository ?? new CatalogRepository<CatalogItem>(_dbContext);
            }
        }
        public ICatalogRepository<CatalogOwner> CatalogOwnerRepository 
        {
            get
            {
                return _catalogOwnerRepository = _catalogOwnerRepository ?? new CatalogRepository<CatalogOwner>(_dbContext);
            }
        }
        public ICatalogRepository<CatalogType> CatalogTypeRepository {
            get
            {
                return _catalogTypeRepository = _catalogTypeRepository ?? new CatalogRepository<CatalogType>(_dbContext);
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            _dbContext.Dispose();
        }
    }
}
