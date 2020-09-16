using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.Storage
{
    public class CatalogDbContext : DbContext, ICatalogDbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogOwner> CatalogOwners { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }

    }
}
