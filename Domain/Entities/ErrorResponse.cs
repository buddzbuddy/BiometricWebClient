using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ErrorResponse
    {
        public bool isCritical { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorString { get; set; }
    }
}
