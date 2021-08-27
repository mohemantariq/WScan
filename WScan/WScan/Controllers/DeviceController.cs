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
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;
        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        public IActionResult GetListofScanners()
        {
            var scanners = _deviceService.GetScanners();
            return Ok(scanners);
        }
    }
}
