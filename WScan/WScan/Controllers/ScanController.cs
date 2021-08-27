using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScan.Service;

namespace WScan.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScanController : Controller
    {
        private readonly IScannerService _scannerService;
        public ScanController(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_scannerService.Scan());
        }
    }
}
