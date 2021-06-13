using System.Threading.Tasks;
using CW_5.Models;

namespace CW_5.Services
{
    public interface IDbService
    {
        Task<int> AddProductToWarehouseAsync(Product_Warehouse productWarehouse);
    }
}