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
        private readonly DBContext _context;

        public ScanController(IScannerService scannerService, DBContext context)
        {
            _scannerService = scannerService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Scan()
        {
            return Ok(await _scannerService.ScanAsync());
        }

        [HttpGet("ScanToBase64")]
        public async Task<IActionResult> ScanToBase64Async()
        {
            return Ok(await _scannerService.ScanToBase64());
        }
    }
}
