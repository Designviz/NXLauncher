# define name of installer
Name "NX Launcher"

OutFile "NXL.0.1.0.Installer.x64.exe"
 
# define installation directory
InstallDir $PROGRAMFILES\DesignVisionaries
 
# For removing Start Menu shortcut in Windows 7
RequestExecutionLevel admin
 

Page directory
Page instfiles

UninstPage uninstConfirm
UninstPage instfiles


# start default section
Section ""
 
    # set the installation directory as the destination for the following actions
    SetOutPath $INSTDIR
 
    File "Newtonsoft.Json.dll"
    File "NXLauncher.deps.json"
    File "NXLauncher.dll"
    File "NXLauncher.exe"
    File "NXLauncher.pdb"
    File "NXLauncher.runtimeconfig.json"

    File "ref\NXLauncher.dll"


    # create the uninstaller
    WriteUninstaller "$INSTDIR\uninstall.exe"
 
    # create a shortcut named "new shortcut" in the start menu programs directory
    # point the new shortcut at the program uninstaller
    CreateShortcut "$SMPROGRAMS\Uninstall.lnk" "$INSTDIR\uninstall.exe"
SectionEnd
 
# uninstaller section start
Section "uninstall"
 
    # Remove the link from the start menu
    Delete "$SMPROGRAMS\Uninstall.lnk"
 
    # Delete the uninstaller
    Delete $INSTDIR\uninstaller.exe
 
    RMDir $INSTDIR
# uninstaller section end
SectionEnd