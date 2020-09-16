using ApiGateway.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGateway.API.Services
{
    public interface ICatalogServices
    {
        Task<List<CatalogItem>> GetCatalogItems();
    }
}
