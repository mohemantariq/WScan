# WScan - Web Document Scanner (Scan from browser)
### free alternative to Dynamic Web TWAIN 
Document Scanning API Your Web Applications (tested on win only)

### How to use it 
* pull the project 
* publish it in file system ( self contained - win 64)
* run install.ps1 scrpit 
 

### List of APIs
- http://localhost:5002/device to get list of available devices
- http://localhost:5002/device/select to select device 
- http://localhost:5002/scan to start scanning and save document as image and return the id in response

you can change port 5002 to any port from appsettings 


 
 
 
### TODO

- [x] select scanner
- [ ] select resolution dpi
- [ ] select color mode 
- [ ] add option to save image or use one image
- [ ] add log 
 
