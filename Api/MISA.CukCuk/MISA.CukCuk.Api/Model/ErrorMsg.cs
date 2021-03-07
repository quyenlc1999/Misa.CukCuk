using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.CukCuk.Api.Model
{
    public class ErrorMsg
    {
        public string devMsg { get; set; }
        public string userMsg { get; set; }
        public string errorCode { get; set; }
        public string moreInfo { get; set; }
        public string traceId { get; set; }
    }
}
