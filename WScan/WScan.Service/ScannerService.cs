using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
        private Microsoft.Extensions.Hosting.IHostingEnvironment _hostingEnvironment;
        public ScannerService(Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

        }
        public Document Scan(string ScannerName)
        {
            throw new NotImplementedException();
        }

        public Document Scan()
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                ImageFile imageFile = ScanDocumnet();

                // Save the image in some path with filename
                var path = @$"{_hostingEnvironment.ContentRootPath}/Upload/{id}.jpeg";

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                // Save image !
                imageFile.SaveFile(path);
                return new Document() { Id = id };
            }
            catch (COMException ex)
            {

                throw;
            }
        }

        private static ImageFile ScanDocumnet()
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
            int resolution = 100;
            int width_pixel = 3510;
            int height_pixel = 5100;
            int color_mode = 1;
            ScannerSettings.AdjustScannerSettings(scannerItem, resolution, 0, 0, width_pixel, height_pixel, 0, 0, color_mode);

            // Retrieve a image in JPEG format and store it into a variable
            var imageFile = (ImageFile)scannerItem.Transfer("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}");
            return imageFile;
        }

        public Document ScanToBase64()
        {
            try
            {
                ImageFile imageFile = ScanDocumnet();
                Byte[] imageBytes = (byte[])imageFile.FileData.get_BinaryData(); // <– Converts the ImageFile to a byte array
                return new Document() { Base64 = Convert.ToBase64String(imageBytes) };
            }
            catch (COMException ex)
            {

                throw;
            }
        }
    }
}
