using Catalog.API.Infrastructure.Repositories;
using Catalog.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.UnitofWorks
{
    public interface IUnitofWork
    {
        ICatalogRepository<CatalogItem> CatalogItemRepository { get;}
        ICatalogRepository<CatalogOwner> CatalogOwnerRepository { get;}
        ICatalogRepository<CatalogType> CatalogTypeRepository { get; }
        void Commit();
        void Rollback();
    }
}
