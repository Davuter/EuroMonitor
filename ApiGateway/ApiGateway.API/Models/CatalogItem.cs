using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Models
{
    public class CatalogItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public int OwnerId { get; set; }
        public CatalogOwner CatalogOwner { get; set; }
        public bool SubscriptedUser { get; set; }
        public int SubscriptedId { get; set; }
    
    }

    public class CatalogOwner
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CatalogType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
