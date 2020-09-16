using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Catalog.API.Infrastructure.UnitofWorks;
using Catalog.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IUnitofWork _unitofwork;
        public CatalogController(IUnitofWork unitofWork)
        {
            _unitofwork = unitofWork;
        }
        [HttpGet]
        [Route("items")]
        [ProducesResponseType(typeof(IEnumerable<CatalogItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]

        public async Task<IActionResult> GetCatalogItemsAsync()
        {
            var model = _unitofwork.CatalogItemRepository.Get(null,null, "CatalogType,CatalogOwner");
            return Ok(model);
        }

        [HttpGet]
        [Route("item/{id}")]
        public async Task<IActionResult> GetCatalogItemAsync(int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }
            var model = _unitofwork.CatalogItemRepository.Get(r=> r.Id == id ,null , "CatalogType,CatalogOwner");
            if (model.Count() == 0)
            {
                return NotFound();
            }
            return Ok(model);
        }
    }
}