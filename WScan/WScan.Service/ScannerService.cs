using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WIA;
using WScan.Shared;
using static System.Net.Mime.MediaTypeNames;

namespace WScan.Service
{
    public class ScannerService : IScannerService
    {
        public Document Scan(string ScannerName)
        {
            throw new NotImplementedException();
        }

        public Document Scan()
        {
            try
            {
                // Create a DeviceManager instance
                var deviceManager = new DeviceManager();
                // Create an empty variable to store the scanner instance
                DeviceInfo firstScannerAvailable = null;

                // Loop through the list of devices to choose the first available
                for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
                {
                    // Skip the device if it's not a scanner
                    if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                    {
                        continue;
                    }

                    firstScannerAvailable = deviceManager.DeviceInfos[i];

                    break;
                }
                  
                // Connect to the first available scanner
                var device = firstScannerAvailable.Connect();

                // Select the scanner
                var scannerItem = device.Items[1];

                // Retrieve a image in JPEG format and store it into a variable
                var imageFile = (ImageFile)scannerItem.Transfer("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}");

                // Save the image in some path with filename
                var path = @$"{System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\{Guid.NewGuid()}.jpeg";

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                // Save image !
                imageFile.SaveFile(path);
                return new Document() { Path = path };
            }
            catch (COMException ex)
            {

                throw;
            }
        }
    }
}
