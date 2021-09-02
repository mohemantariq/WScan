using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WScan.Shared;

namespace WScan.Service.Models
{
    public class DocumentScanResponse
    {
        public Document Document { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
