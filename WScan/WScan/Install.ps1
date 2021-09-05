## if you face unauthorized issue please run the below command as Administrator
#Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine

param([switch]$Elevated)

function Test-Admin {
    $currentUser = New-Object Security.Principal.WindowsPrincipal $([Security.Principal.WindowsIdentity]::GetCurrent())
    $currentUser.IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)
}

if ((Test-Admin) -eq $false)  {
    if ($elevated) {
        # tried to elevate, did not work, aborting
    } else {
        Start-Process powershell.exe -Verb RunAs -ArgumentList ('-noprofile -noexit -file "{0}" -elevated' -f ($myinvocation.MyCommand.Definition))
    }
    exit
}
#Set-Location -Path 'c:\WScan'

sc.exe delete WScanService;
sc.exe create WScanService binPath= c:\WScan\WScan.API.exe start= auto ; sc.exe start WScanService