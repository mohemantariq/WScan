using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScan.Service;

namespace WScan.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScannerController : Controller
    {
        private readonly IScannerService _scannerService;
        public ScannerController(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        [HttpGet]
        public IActionResult GetListofScanners()
        {
            var scanners = _scannerService.GetScanners();
            return Ok(scanners);
        }
    }
}
