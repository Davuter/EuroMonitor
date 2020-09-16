using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.Storage
{
    public interface ICatalogDbContext
    {
        DbSet<CatalogItem> CatalogItems { get; set; }
        DbSet<CatalogOwner> CatalogOwners { get; set; }
        DbSet<CatalogType> CatalogTypes { get; set; }

    }
}
