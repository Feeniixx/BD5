using System;
using System.Threading.Tasks;
using CW_5.Models;
using CW_5.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_5.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IDbService DbService;

        public WarehouseController(IDbService DbService)
        {
            this.DbService = DbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouse([FromBody] Product_Warehouse productWarehouse)
        {
            int idProductWarehouse;
            try { idProductWarehouse = await DbService.AddProductToWarehouseAsync(productWarehouse); }

            catch (Exception e) { return NotFound(e.Message); }
            return Ok($"Dodane! ID: {idProductWarehouse}!");
        }


    }
}
