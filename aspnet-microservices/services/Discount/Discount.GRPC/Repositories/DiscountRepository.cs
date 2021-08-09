using Dapper;
using Discount.GRPC.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.GRPC.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("Insert into coupon(ProductName, Description, Amount) values (@productname, @description, @amount)", new { productname = coupon.ProductName, description = coupon.Description, amount = coupon.Amount });

            if (affected > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("delete from coupon where ProductName = @productname", new { productname = productName});

            if (affected > 0)
                return true;
            else
                return false;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("Select * from coupon where productname=@productname", new { productname = productName });

            if (coupon == null)
                return new Coupon() { ProductName = "No Discount", Amount = 0, Description = "No Dicount Desc" };

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("update coupon set ProductName = @productname, description = @description, amount = @amount where id = @id", new { productname = coupon.ProductName, description = coupon.Description, amount = coupon.Amount, id = coupon.Id });

            if (affected > 0)
                return true;
            else
                return false;
        }
    }
}
