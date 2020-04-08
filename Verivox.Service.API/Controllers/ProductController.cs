using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using Verivox.Common;
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
            var model = new CommonResponse<List<ProductResult>>();
            try
            {
                model.Result = _productService.OfferProductsByConsumption(new ProductSearch
                {
                    Consumption = Consumption
                });
            }
            catch (Exception)
            {
                model.IsError = true;
                model.Message = "Occur an error, please try later!";
            }
            return Ok(model);
        }
    }
}
