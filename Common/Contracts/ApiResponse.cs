using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public class ApiResponse
    {
        public int HttpStatusCode { get; set; }
        public string? Message { get; set; }
        public bool? IsSuccess { get; set; }
    }
}
