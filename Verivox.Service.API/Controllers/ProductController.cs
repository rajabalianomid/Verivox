using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Verivox.Common;
using Verivox.Common.Plugins;
using Verivox.Domain;
using Verivox.Domain.Search;
using Verivox.Service.API.Models;

namespace Verivox.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IWebHelper _webHelper;

        public ProductController(IProductService productService, IWebHelper webHelper)
        {
            this._productService = productService;
            this._webHelper = webHelper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductResult>), (int)HttpStatusCode.OK)]
        public IActionResult GetAll(int Consumption)
        {
            var result = _productService.OfferProductsByConsumption(new ProductSearch
            {
                Consumption = Consumption
            });
            return Ok(result);
        }
    }
}
