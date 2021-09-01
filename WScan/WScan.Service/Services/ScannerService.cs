using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using WIA;
using WScan.Service.Services;
using WScan.Shared;

namespace WScan.Service
{
    public class ScannerService : IScannerService
    {
        private Microsoft.Extensions.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly IOptionService _optionService;
        public ScannerService(Microsoft.Extensions.Hosting.IHostingEnvironment hostingEnvironment, IOptionService optionService)
        {
            _hostingEnvironment = hostingEnvironment;
            _optionService = optionService;

        }
        public async Task<Document> ScanAsync(string ScannerName)
        {
            await CheckForScanner();
            throw new NotImplementedException();
        }

        private async Task CheckForScanner()
        {
            if (string.IsNullOrEmpty(await _optionService.GetOptionValue("SelectedScanner")))
                throw new NullReferenceException("pelase select scanner first ");
        }

        public async Task<Document> ScanAsync()
        {
            await CheckForScanner();

            try
            {
                string id = Guid.NewGuid().ToString();
                ImageFile imageFile = await ScanDocumnetAsync();

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

        private async Task<ImageFile> ScanDocumnetAsync()
        {
            var selectedScannerId = await _optionService.GetOptionValue("SelectedScanner");
            // Create a DeviceManager instance
            var deviceManager = new DeviceManager();

            // Create an empty variable to store the scanner instance
            DeviceInfo selectedScanner = null;

            // Loop through the list of devices to choose the first available
            for (int i = 1; i <= deviceManager.DeviceInfos.Count; i++)
            {
                // Skip the device if it's not a scanner
                if (deviceManager.DeviceInfos[i].Type != WiaDeviceType.ScannerDeviceType)
                {
                    continue;
                }
                if (deviceManager.DeviceInfos[i].Properties["Name"].get_Value() == selectedScannerId)
                    selectedScanner = deviceManager.DeviceInfos[i];

                break;
            }

            // Connect to the first available scanner
            var device = selectedScanner.Connect();

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

        public async Task<Document> ScanToBase64()
        {
            await CheckForScanner();

            try
            {
                ImageFile imageFile = await ScanDocumnetAsync();
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
