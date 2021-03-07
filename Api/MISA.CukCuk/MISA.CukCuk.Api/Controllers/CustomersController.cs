using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using MISA.CukCuk.Api.Model;
using System.Data.SqlClient;
using System.Data;
using MySqlConnector;
using MISA.CukCuk.Api.Services;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns>List tất cả danh sách khách hàng</returns>
        /// CreatedBy: LCQUYEN (28/02/2021)
        [HttpGet]
        public IActionResult getCustomers()
        {
            var customerService = new CustomerService();
            var customers = customerService.getCustomers().ToList();
            if(customers.Count > 0)
            {
                return StatusCode(200, customers);
            }
            else
            {
                return StatusCode(204, customers);
            }
        }
        /// <summary>
        /// Thêm mới thông tin khách hàng vào CSDL  
        /// </summary>
        /// <param name="customer">Thực thể khách hàng</param>
        /// <returns>Trả về mã các mã trạng thái cho client (201-thêm thành công...)    </returns>
        /// CreatedBy: LCQUYEN (28/02/2021)
        [HttpPost]
        public IActionResult PostCustomer(Customer customer)
        {
            //Khai báo chuỗi kết nối CSDL
            var connString = @"Host=47.241.69.179; Port=3306; Database=MF749_LCQUYEN_CukCuk; User Id=dev; Password=12345678";
            //Khởi tạo đối tượng kết nối
            IDbConnection dbConnection = new MySqlConnection(connString);
            var errorMsg = new ErrorMsg();
            // Validate dữ liệu bắt buộc nhập
            if(customer.CustomerCode == null)
            {
                return StatusCode(404, "Bạn phải nhập thông tin mã khách hàng");
            }
            // Validate dữ liệu không được phép trùng
            // - kiểm tra mã khách hàng tồn tại trong db chưa ?
            var sql = $"SELECT CustomerCode FROM Customer AS c WHERE c.CustomerCode='{customer.CustomerCode}'";
            var customerCode = dbConnection.Query<string>(sql).FirstOrDefault();
            if(customerCode != null)
            {
                errorMsg.devMsg = "Database thiết lập Unique cho mã khách hàng";
                errorMsg.userMsg = "Mã khách hàng không được phép trùng";
                return StatusCode(400, errorMsg);
            }
            //-Kiểm tra số điện thoại tồn tại trong database chưa ?
            var sqlPhoneNumber = $"SELECT PhoneNumber FROM Customer AS c WHERE c.PhoneNumber='{customer.PhoneNumber}'";
            var phoneNumber = dbConnection.Query<String>(sqlPhoneNumber).FirstOrDefault();  
            if(phoneNumber != null) {
                errorMsg.devMsg = "Database thiết lập Unique cho số điện thoại";
                errorMsg.userMsg = "Số điện thoại đã tồn tại trong sơ sở dữ liệu";
                return StatusCode(400, errorMsg);
            }

         var sqlInsertCustomer = $"INSERT INTO Customer (CustomerId,CustomerCode, FullName," +
                $" Gender, MemberCardCode, CustomerGroupId, PhoneNumber, DateOfBirth, CompanyName, " +
                $"CompanyTaxCode, Email, Address, Note, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)" +
                $" VALUE('{customer.CustomerId}','{customer.CustomerCode}','{customer.FullName}'," +
                $"'{customer.Gender}','{customer.MemberCardCode}','{customer.CustomerGroupId}'," +
                $"'{customer.PhoneNumber}','{customer.DateOfBirth}','{customer.CompanyName}','{customer.CompanyTaxCode}'," +
                $"'{customer.Email}','{customer.Address}','{customer.Note}','{customer.CreatedDate}','{customer.CreatedBy}'," +
                $"'{customer.ModifiedDate}', '{customer.ModifiedBy}')";
 
            //Thực thi câu lệnh truy vấn    
            var res = dbConnection.Execute("Proc_InsertCustomer", param:customer,commandType:CommandType.StoredProcedure);
            if (res > 0)
            {
                return StatusCode(201, res);
            }
            else
            {
                return StatusCode(200, "Không có bản ghi nào được thêm mới!");
            }
             //    return Ok(1);
        }
    }
}
