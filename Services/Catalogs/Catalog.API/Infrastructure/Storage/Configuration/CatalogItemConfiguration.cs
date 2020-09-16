using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.Storage.Configuration
{
    public class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("CatalogItem");
            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.CatalogType).WithMany().HasForeignKey(r => r.CatalogTypeId);
            builder.HasOne(k => k.CatalogOwner).WithMany().HasForeignKey(r => r.OwnerId);
        }
    }
}
