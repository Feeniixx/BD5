using System;
using System.Threading.Tasks;
using CW_5.Exceptions;
using CW_5.Models;


namespace CW_5.Services
{
    public class DbService : IDbService
    {
        private readonly string connectionString = @"Data Source=db-mssql;Initial Catalog=s19240;Integrated Security=True";

        public async Task<int> AddProductToWarehouseAsync(Product_Warehouse productWarehouse)
        {
            using var connection = new SqlConnection(connectionString);
            using var cmd = new SqlCommand();

            cmd.Connection = connection;
            await connection.OpenAsync();

            cmd.CommandText = "SELECT TOP 1 [Order].IdOrder FROM [Order] " +
                "LEFT JOIN Product_Warehouse ON [Order].IdOrder = Product_Warehouse.IdOrder " +
                "WHERE [Order].IdProduct = @IdProduct " +
                "AND [Order].Amount = @Amount " +
                "AND Product_Warehouse.IdProductWarehouse IS NULL " +
                "AND [Order].CreatedAt < @CreatedAt";

            cmd.Parameters.AddWithValue("IdProduct", productWarehouse.IdProduct);
            cmd.Parameters.AddWithValue("Amount", productWarehouse.Amount);
            cmd.Parameters.AddWithValue("CreatedAt", productWarehouse.CreatedAt);


            var reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows) throw new NoRowsException("Invalid parameter: there is no order to fullfill!");

            await reader.ReadAsync();
            int idOrder = int.Parse(reader["IdOrder"].ToString());
            await reader.CloseAsync();

            cmd.Parameters.Clear();

            cmd.CommandText = "SELECT Price FROM Product WHERE IdProduct = @IdProduct";
            cmd.Parameters.AddWithValue("IdProduct", productWarehouse.IdProduct);

            reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows) throw new NoRowsException("Invalid parameter: provided IdProduct does not exist!");

            await reader.ReadAsync();
            double price = double.Parse(reader["Price"].ToString());
            await reader.CloseAsync();

            cmd.Parameters.Clear();

            cmd.CommandText = "SELECT IdWarehouse FROM Warehouse WHERE IdWarehouse = @IdWarehouse";
            cmd.Parameters.AddWithValue("IdWarehouse", productWarehouse.IdWarehouse);

            reader = await cmd.ExecuteReaderAsync();

            if (!reader.HasRows) throw new NoRowsException("Invalid parameter: provided IdWarehouse does not exist!");

            await reader.CloseAsync();
            cmd.Parameters.Clear();


            var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
            cmd.Transaction = transaction;

            try
            {
                cmd.CommandText = "UPDATE [Order] SET FulfilledAt = @CreatedAt WHERE IdOrder = @IdOrder";
                cmd.Parameters.AddWithValue("CreatedAt", productWarehouse.CreatedAt);
                cmd.Parameters.AddWithValue("IdOrder", idOrder);

                int rowsUpdated = await cmd.ExecuteNonQueryAsync();

                if (rowsUpdated < 1) throw new NoResultException();

                cmd.Parameters.Clear();

                cmd.CommandText = "INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                    $"VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Amount*{price}, @CreatedAt)";

                cmd.Parameters.AddWithValue("IdWarehouse", productWarehouse.IdWarehouse);
                cmd.Parameters.AddWithValue("IdProduct", productWarehouse.IdProduct);
                cmd.Parameters.AddWithValue("IdOrder", idOrder);
                cmd.Parameters.AddWithValue("Amount", productWarehouse.Amount);
                cmd.Parameters.AddWithValue("CreatedAt", productWarehouse.CreatedAt);

                int rowsInserted = await cmd.ExecuteNonQueryAsync();

                if (rowsInserted < 1) throw new NoResultException();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new Exception404("Coś nie tak(404)");
            }

            cmd.Parameters.Clear();

            cmd.CommandText = "SELECT TOP 1 IdProductWarehouse FROM Product_Warehouse ORDER BY IdProductWarehouse DESC";

            reader = await cmd.ExecuteReaderAsync();

            await reader.ReadAsync();
            int idProductWarehouse = int.Parse(reader["IdProductWarehouse"].ToString());
            await reader.CloseAsync();

            await connection.CloseAsync();

            return idProductWarehouse;

        }
            
    }

 
}
        

    
