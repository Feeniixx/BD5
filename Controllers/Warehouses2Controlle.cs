using System;
using System.Threading.Tasks;
using CW_5.Models;
using CW_5.Services;

using Microsoft.AspNetCore.Mvc;

namespace CW_5.Controllers
{
    [Route("api/warehouses2")]
    [ApiController]
    public class Warehouse2Controller : ControllerBase
    {
        private readonly IDbProcedureService DbProcedureService;

        public Warehouse2Controller(IDbProcedureService DbProcedureService)
        {
            this.DbProcedureService = DbProcedureService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouse([FromBody] Product_Warehouse productWarehouse)
        {
            int idProductWarehouse;
            try { idProductWarehouse = await DbProcedureService.AddProductToWarehouseAsync(productWarehouse); }
            catch (Exception e) { return NotFound(e.Message); }
            return Ok($"Dodane! ID: {idProductWarehouse}!");
        }
    }
}