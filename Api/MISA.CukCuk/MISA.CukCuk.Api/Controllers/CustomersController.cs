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
            var customerService = new CustomerService();
            var customerInsert = customerService.InsertCustomer(customer);
            if (customerInsert.Sussess == true && customerInsert.Data != null)
            {
                return StatusCode(201, customerInsert);
            }
            else
            {
                return StatusCode(204, customerInsert);
            }
        }
        [HttpPut]
        public IActionResult PutCustomer(Customer customer)
        {
            var customerService = new CustomerService();
            var customerUpdate = customerService.UpdateCustomer(customer);
            if(customerUpdate.Sussess == true && customerUpdate.Data != null)
            {
                return StatusCode(200, customerUpdate);
            }
            else
            {
                return StatusCode(404, customerUpdate);
            }
        }
    }
}
