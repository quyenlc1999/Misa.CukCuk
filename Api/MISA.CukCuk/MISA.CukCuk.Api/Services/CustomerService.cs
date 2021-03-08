
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
        //    var sql = $"SELECT * FROM Customer ORDER BY ModifiedDate DESC";
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType:CommandType.StoredProcedure);
            //Trả về kết quả cho client
            return customers;
        }

        /// <summary>
        /// Kiểm tra khóa chính đã tồn tại trong db hay chưa
        /// CreatedBy: LCQUYEN (28/02/2021)
        /// </summary>
        /// <param name="customerId">Khóa chính cần kiểm tra</param>
        /// <returns>
        /// true: đã tồn tại
        /// false: chưa tồn tại
        /// </returns>
        public bool CheckCustomerIdExits(string customerId)
        {
            IDbConnection dbConnection = new MySqlConnection(connString);
            var sql = $"SELECT CustomerId FROM Customer AS c WHERE c.CustomerId='{customerId}'";
            var customerIdExits = dbConnection.Query<string>(sql).FirstOrDefault();
            if (customerIdExits != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Kiểm tra mã khách hàng đã tồn tại trong db hay chưa
        /// CreatedBy: LCQUYEN (28/02/2021)
        /// </summary>
        /// <param name="customerCode">Mã khách hàng cần kiểm tra</param>
        /// <returns>
        /// true: đã tồn tại
        /// false: chưa tồn tại
        /// </returns>
        public bool CheckCustomerCodeExits(string customerCode)
        {
            IDbConnection dbConnection = new MySqlConnection(connString);
            var sql = $"SELECT CustomerCode FROM Customer AS c WHERE c.CustomerCode='{customerCode}'";
            var customerCodeExits = dbConnection.Query<string>(sql).FirstOrDefault();
            if(customerCodeExits != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại trong db hay chưa
        /// CreatedBy: LCQUYEN (28/02/2021)
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại cần kiểm tra</param>
        /// <returns>
        /// true: đã tồn tại
        /// false: chưa tồn tại
        /// </returns>
        public bool CheckPhoneNumberExits(string phoneNumber)
        {
            IDbConnection dbConnection = new MySqlConnection(connString);
            var sql = $"SELECT PhoneNumber FROM Customer AS c WHERE c.PhoneNumber='{phoneNumber}'";
            var phoneNumberExits = dbConnection.Query<string>(sql).FirstOrDefault();
            if (phoneNumberExits != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra mã thẻ thành viên đã tồn tại trong db hay chưa
        /// CreatedBy: LCQUYEN (28/02/2021)
        /// </summary>
        /// <param name="memberCardCode">Mã thẻ thành viên cần kiểm tra</param>
        /// <returns>
        /// true: đã tồn tại
        /// false: chưa tồn tại
        /// </returns>
        public bool CheckMemberCardCodeExits(string memberCardCode)
        {
            IDbConnection dbConnection = new MySqlConnection(connString);
            var sql = $"SELECT MemberCardCode FROM Customer AS c WHERE c.MemberCardCode='{memberCardCode}'";
            var memberCardCodeExits = dbConnection.Query<string>(sql).FirstOrDefault();
            if (memberCardCodeExits != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Service thêm mới khách hàng
        /// CreatedBy: LCQUYEN (28/02/2021)
        /// </summary>
        /// <param name="customer">Khách hàng thêm mới</param>
        /// <returns>Số bản ghi thêm mới thành công hoặc thất bại</returns>
        public ServiceResult InsertCustomer(Customer customer)
        {
            //Khai báo chuỗi kết nối CSDL
        //    var connString = @"Host=47.241.69.179; Port=3306; Database=MF749_LCQUYEN_CukCuk; User Id=dev; Password=12345678";
            //Khởi tạo đối tượng kết nối
            IDbConnection dbConnection = new MySqlConnection(connString);
            var errorMsg = new ErrorMsg();
            var serviceResult = new ServiceResult();
            // Validate dữ liệu bắt buộc nhập
            if (customer.CustomerCode == null || customer.CustomerCode == "")
            {
                errorMsg.devMsg = "Database establish CustomerCode not null";
                errorMsg.userMsg = "Mã khách hàng không được để trống";
                serviceResult.Sussess = false;
                serviceResult.Data = errorMsg;
                return serviceResult;
            }

            if (customer.FullName == null || customer.FullName == "")
            {
                errorMsg.devMsg = "Database establish FullName not null";
                errorMsg.userMsg = "Tên khách hàng không được để trống";
                serviceResult.Sussess = false;
                serviceResult.Data = errorMsg;
                return serviceResult;
            }

            if (customer.PhoneNumber == null || customer.PhoneNumber == "")
            {
                errorMsg.devMsg = "Da tabase establish PhoneNumber not null";
                errorMsg.userMsg = "Số điện thoại không được để trống";
                serviceResult.Sussess = false;
                serviceResult.Data = errorMsg;
                return serviceResult;
            }

            // Validate dữ liệu không được phép trùng
            // - kiểm tra mã khách hàng tồn tại trong db chưa ?
            var customerCodeExits = CheckCustomerCodeExits(customer.CustomerCode);
            if (customerCodeExits == true)
            {
                errorMsg.devMsg = "Database establish CustomerCode unique";
                errorMsg.userMsg = "Mã khách hàng đã tồn tại";
                serviceResult.Sussess = false;
                serviceResult.Data = errorMsg;
                return serviceResult;
            }
            //-Kiểm tra số điện thoại tồn tại trong database chưa ?
            var phoneNumberCodeExits = CheckPhoneNumberExits(customer.PhoneNumber);
            if (phoneNumberCodeExits == true)
            {
                errorMsg.devMsg = "Database establish PhoneNumber unique";
                errorMsg.userMsg = "Số điện thoại đã tồn tại";
                serviceResult.Sussess = false;
                serviceResult.Data = errorMsg;
                return serviceResult;
            }
            
            /*var sqlInsertCustomer = $"INSERT INTO Customer (CustomerId,CustomerCode, FullName," +
                   $" Gender, MemberCardCode, CustomerGroupId, PhoneNumber, DateOfBirth, CompanyName, " +
                   $"CompanyTaxCode, Email, Address, Note, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)" +
                   $" VALUE('{customer.CustomerId}','{customer.CustomerCode}','{customer.FullName}'," +
                   $"'{customer.Gender}','{customer.MemberCardCode}','{customer.CustomerGroupId}'," +
                   $"'{customer.PhoneNumber}','{customer.DateOfBirth}','{customer.CompanyName}','{customer.CompanyTaxCode}'," +
                   $"'{customer.Email}','{customer.Address}','{customer.Note}','{customer.CreatedDate}','{customer.CreatedBy}'," +
                   $"'{customer.ModifiedDate}', '{customer.ModifiedBy}')";
    */
            //Thực thi câu lệnh truy vấn    
            var res = dbConnection.Execute("Proc_InsertCustomer", param: customer, commandType: CommandType.StoredProcedure);
            if (res > 0)
            {
                errorMsg.devMsg = $"Add sussess {res}";
                errorMsg.userMsg = $"Thêm mới thành công {res} bản ghi";
                serviceResult.Sussess = true;
                serviceResult.Data = errorMsg;
                return serviceResult;
            }
            else
            {
                errorMsg.devMsg = $"Add failed {res}";
                errorMsg.userMsg = "Thêm mới thất bại ";
                serviceResult.Sussess = false;
                serviceResult.Data = errorMsg;
                return serviceResult;
            }
            //    return Ok(1);
        }
        /// <summary>
        /// Service sửa thông tin khách hàng
        /// CreatedBy: LCQUYEN (28/02/2021)
        /// </summary>
        /// <param name="customer">Thông tin khách hàng chỉnh sửa</param>
        /// <returns>Số bản ghi cập nhật thành công hoặc thất bại</returns>
        public ServiceResult UpdateCustomer(Customer customer)
        {
            IDbConnection dbConnection = new MySqlConnection(connString);
            var errorMsg = new ErrorMsg();
            var serviceResult = new ServiceResult();
            var customerIdCheck = customer.CustomerId;
            var customerId = CheckCustomerIdExits(customerIdCheck.ToString());
       //     var customerCode = CheckCustomerCodeExits(customer.CustomerCode);
            if(customerId == true)
            {
            /*   var sqlUpdate = $"UPDATE Customer SET CustomerCode='{customer.CustomerCode}' AND FullName='{customer.FullName}' AND Gender='{customer.Gender}' AND MemberCardCode='{customer.MemberCardCode}'" +
               $"AND CustomerGroupId='{customer.CustomerGroupId}' AND PhoneNumber = '{customer.PhoneNumber}' AND DateOfBirth = '{customer.DateOfBirth}' " +
               $"AND CompanyName = '{customer.CompanyName}' AND CompanyTaxCode = '{customer.CompanyTaxCode}' AND Email='{customer.Email}'" +
               $"AND Address = '{customer.Address}' AND Note = '{customer.Note}' AND CreatedDate='{customer.CreatedDate}' AND CreatedBy='{customer.CreatedBy}' AND ModifiedDate = {customer.ModifiedDate}" +
               $"AND ModifiedBy = '{customer.ModifiedBy}'" +    
               $"WHERE CustomerId = '{customer.CustomerId}'";
            */
               var res = dbConnection.Execute("Proc_UpdateCustomer", param: customer, commandType: CommandType.StoredProcedure);           
               if (res > 0)
                {
                    errorMsg.devMsg = $"Update sussess {res}";
                    errorMsg.userMsg = $"Sửa thành công {res} bản ghi";
                    serviceResult.Sussess = true;
                    serviceResult.Data = errorMsg;
                    return serviceResult;
                }
                else
                {
                    errorMsg.devMsg = "Update failed";
                    errorMsg.userMsg = "Sửa thất bại ";
                    serviceResult.Sussess = false;
                    serviceResult.Data = errorMsg;
                    return serviceResult;
                }
            }
            else
            {
                 errorMsg.devMsg = "CustomerId and CustomerCode not exit";
                 errorMsg.userMsg = "Mâ khách hàng không tồn tại.";
                 serviceResult.Sussess = false;
                 serviceResult.Data = errorMsg;
                 return serviceResult;
            }
        }
        }
    }   