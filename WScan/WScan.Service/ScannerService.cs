using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WScan.Shared;
using WIA;

namespace WScan.Service
{
    public class ScannerService : IScannerService
    {
        public List<Scanner> GetScanners()
        {
            var Scanners = new List<Scanner>();
            // Create a DeviceManager instance
            var deviceManager = new DeviceManager();

            // Loop through the list of devices
            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                // Skip the device if it's not a scanner
                if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                {
                    continue;
                }
                Scanners.Add(new Scanner
                {
                    Name = deviceManager.DeviceInfos[i].Properties["Name"].get_Value()
                });


            }
            return Scanners;
        }
    }
}
