#define MyAppName "RUKN Search"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "RUKN BIM"
#define MyAppURL "https://www.ruknbim.com"
#define MyAppExeName "RUKN_Search_Setup.exe"

[Setup]
AppId={{7B3D6C4A-51E2-4F1B-9C82-8A3D0F1E4A72}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={userappdata}\Autodesk\ApplicationPlugins\RUKN.Search.bundle
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=Output
OutputBaseFilename=RUKN_Search_Setup
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=lowest

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "..\RUKN.Search.bundle\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Code]
// Close Navisworks (Roamer.exe) if it is running before installation
function InitializeSetup(): Boolean;
var
  ErrorCode: Integer;
begin
  Result := True;
  Exec('taskkill.exe', '/F /IM Roamer.exe', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
end;
