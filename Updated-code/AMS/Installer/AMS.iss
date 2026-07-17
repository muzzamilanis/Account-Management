; Inno Setup script for Jaini Auto Accounts Manager
; Build with: ISCC.exe AMS.iss

#define MyAppName "Jaini Auto Accounts Manager"
#define MyAppVersion "1.0.1"
#define MyAppPublisher "Jaini Motors"
#define MyAppExeName "AMS.exe"

[Setup]
AppId={{7E6C9C6B-6E7D-4F7C-9C1D-9A6A9E9A9A11}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=.
OutputBaseFilename=JainiAutoAccountsManager_Setup
Compression=lzma
SolidCompression=yes
SetupIconFile=..\Autos_Accounts.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
ArchitecturesInstallIn64BitMode=x64compatible
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\bin\Release\net48\AMS.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\net48\AMS.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\net48\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\bin\Release\net48\x86\SQLite.Interop.dll"; DestDir: "{app}\x86"; Flags: ignoreversion
Source: "..\bin\Release\net48\x64\SQLite.Interop.dll"; DestDir: "{app}\x64"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\Uninstall {#MyAppName}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent
