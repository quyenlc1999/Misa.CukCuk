using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Model
{
    /// <summary>
    /// Thông tin trả về cho Api
    /// CreatedBy: LCQUYEN(08/03/2021)
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Trạng thái service (true - thành công ; false-thất bại)
        /// </summary>
        public bool Sussess { get; set; }
        /// <summary>
        /// Dữ liệu trả về cHo API
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Mã code trả về của Misa
        /// </summary>
        public string MISACode { get; set; }
    }
}
