using System.Threading.Tasks;
using CW_5.Models;

namespace CW_5.Services
{
    public interface IDbProcedureService
    {
        Task<int> AddProductToWarehouseAsync(Product_Warehouse productWarehouse);
    }
}