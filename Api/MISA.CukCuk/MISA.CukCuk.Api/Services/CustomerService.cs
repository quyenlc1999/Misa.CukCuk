
using Dapper;
using MISA.CukCuk.Api.Model;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Services
{
    public class CustomerService
    {
        public string connString = @"Host=47.241.69.179; Port=3306;Database=MF749_LCQUYEN_CukCuk;User Id=dev;Password=12345678";
        public IEnumerable<Customer> getCustomers()
        {
          //  var connString = @"Host=47.241.69.179; Port=3306;Database=MF749_LCQUYEN_CukCuk;User Id=dev;Password=12345678";
            //Khởi tạo đối tượng kết nối
            IDbConnection dbConnection = new MySqlConnection(connString);
            //Thực thi câu lệnh truy vấn
            var sql = $"SELECT * FROM Customer ORDER BY ModifiedDate DESC";
            var customers = dbConnection.Query<Customer>(sql, commandType:CommandType.Text);
            //Trả về kết quả cho client
            return customers;
        }
    }
}