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
                ImageFile imageFile = ScanDocumnet();

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
        private static void SetWIAProperty(IProperties properties, object propName, object propValue)
        {
            Property prop = properties.get_Item(ref propName);
            prop.set_Value(ref propValue);
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
            AdjustScannerSettings(scannerItem, resolution, 0, 0, width_pixel, height_pixel, 0, 0, color_mode);

            // Retrieve a image in JPEG format and store it into a variable
            var imageFile = (ImageFile)scannerItem.Transfer("{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}");
            return imageFile;
        }

        public Document ScanToBase64()
        {
            try
            {


                ImageFile imageFile = ScanDocumnet();


                //this should create an image file in memory
                //process image:
                //one would do image processing here
                Byte[] imageBytes = (byte[])imageFile.FileData.get_BinaryData(); // <– Converts the ImageFile to a byte array
                string base64String = Convert.ToBase64String(imageBytes);




                return new Document() { Base64 = base64String };
            }
            catch (COMException ex)
            {

                throw;
            }
        }
        private static void AdjustScannerSettings(IItem scannerItem, int scanResolutionDPI, int scanStartLeftPixel, int scanStartTopPixel, int scanWidthPixels, int scanHeightPixels, int brightnessPercents, int contrastPercents, int colorMode)
        {
            const string WIA_SCAN_COLOR_MODE = "6146";
            const string WIA_HORIZONTAL_SCAN_RESOLUTION_DPI = "6147";
            const string WIA_VERTICAL_SCAN_RESOLUTION_DPI = "6148";
            const string WIA_HORIZONTAL_SCAN_START_PIXEL = "6149";
            const string WIA_VERTICAL_SCAN_START_PIXEL = "6150";
            const string WIA_HORIZONTAL_SCAN_SIZE_PIXELS = "6151";
            const string WIA_VERTICAL_SCAN_SIZE_PIXELS = "6152";
            const string WIA_SCAN_BRIGHTNESS_PERCENTS = "6154";
            const string WIA_SCAN_CONTRAST_PERCENTS = "6155";
            SetWIAProperty(scannerItem.Properties, "4104", 24);
            SetWIAProperty(scannerItem.Properties, WIA_HORIZONTAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            SetWIAProperty(scannerItem.Properties, WIA_VERTICAL_SCAN_RESOLUTION_DPI, scanResolutionDPI);
            //SetWIAProperty(scannerItem.Properties, WIA_HORIZONTAL_SCAN_START_PIXEL, scanStartLeftPixel);
            //SetWIAProperty(scannerItem.Properties, WIA_VERTICAL_SCAN_START_PIXEL, scanStartTopPixel);
            //SetWIAProperty(scannerItem.Properties, WIA_HORIZONTAL_SCAN_SIZE_PIXELS, scanWidthPixels);
            //SetWIAProperty(scannerItem.Properties, WIA_VERTICAL_SCAN_SIZE_PIXELS, scanHeightPixels);
            SetWIAProperty(scannerItem.Properties, WIA_SCAN_BRIGHTNESS_PERCENTS, brightnessPercents);
            SetWIAProperty(scannerItem.Properties, WIA_SCAN_CONTRAST_PERCENTS, contrastPercents);
            SetWIAProperty(scannerItem.Properties, WIA_SCAN_COLOR_MODE, colorMode);
        }
    }
}
