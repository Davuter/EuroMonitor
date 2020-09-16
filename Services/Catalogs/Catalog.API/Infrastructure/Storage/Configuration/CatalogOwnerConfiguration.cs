using Catalog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Catalog.API.Infrastructure.Storage.Configuration
{
    public class CatalogOwnerConfiguration : IEntityTypeConfiguration<CatalogOwner>
    {
        public void Configure(EntityTypeBuilder<CatalogOwner> builder)
        {
            builder.ToTable("CatalogOwner");
            builder.HasKey(r => r.Id);
        }
    }
}
