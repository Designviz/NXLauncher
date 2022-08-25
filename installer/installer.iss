﻿; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "NX Launcher"
#define MyAppVersion "0.1.0"
#define MyAppPublisher "Design Visionaries"
#define MyAppURL "http://designvisionaries.com/"
#define MyAppExeName "NXLauncher.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{B839E775-6ADC-4499-ABA1-4A6A4AEAECAD}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\Designviz NXLauncher
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputBaseFilename=NXL.0.1.0.Installer.x64
SetupIconFile=D:\Repos\NXLauncher\bin\Debug\net6.0-windows\resources\favicon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\NXLauncher.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\NXLauncher.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\NXLauncher.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\NXLauncher.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\NXLauncher.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files
Source: "D:\Repos\NXLauncher\bin\Debug\net6.0-windows\Prerequisites\windowsdesktop-runtime-6.0.8-win-x64.exe"; DestDir: "{app}\Prerequisites"; Flags: ignoreversion

[Icons]
Name: "{autoprograms}\DesignVisionaries\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\Prerequisites\windowsdesktop-runtime-6.0.8-win-x64.exe"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait skipifsilent
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

