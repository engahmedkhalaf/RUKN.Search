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
DisableWelcomePage=no
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
var
  TopPanel: TPanel;
  PublisherLabel: TNewStaticText;
  AppNameLabel: TNewStaticText;
  MiddlePanel: TPanel;
  BottomPanel: TPanel;
  InstallButton: TButton;
  TermsButton: TButton;
  CancelBtn: TButton;
  SuccessLabel: TNewStaticText;

// Close Navisworks (Roamer.exe) if it is running before installation
function InitializeSetup(): Boolean;
var
  ErrorCode: Integer;
begin
  Result := True;
  Exec('taskkill.exe', '/F /IM Roamer.exe', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
end;

procedure InstallButtonClick(Sender: TObject);
begin
  WizardForm.NextButton.OnClick(WizardForm);
end;

procedure TermsButtonClick(Sender: TObject);
var
  ErrorCode: Integer;
begin
  ShellExec('open', 'https://www.ruknbim.com', '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
end;

procedure CustomCancelButtonClick(Sender: TObject);
begin
  WizardForm.Close;
end;

procedure InitializeWizard;
begin
  // Adjust form dimensions
  WizardForm.ClientWidth := 670;
  WizardForm.ClientHeight := 450;

  // Hide default wizard form notebook and standard buttons
  WizardForm.OuterNotebook.Visible := False;
  WizardForm.BackButton.Visible := False;
  WizardForm.NextButton.Visible := False;
  WizardForm.CancelButton.Visible := False;

  // Create Top Banner Panel
  TopPanel := TPanel.Create(WizardForm);
  TopPanel.Parent := WizardForm;
  TopPanel.Left := 0;
  TopPanel.Top := 0;
  TopPanel.Width := WizardForm.ClientWidth;
  TopPanel.Height := 120;
  TopPanel.BevelOuter := bvNone;
  TopPanel.Color := clBlack;

  // Create Publisher Label (RUKNBIM)
  PublisherLabel := TNewStaticText.Create(WizardForm);
  PublisherLabel.Parent := TopPanel;
  PublisherLabel.Caption := 'RUKNBIM';
  PublisherLabel.Font.Name := 'Segoe UI';
  PublisherLabel.Font.Size := 20;
  PublisherLabel.Font.Style := [fsBold];
  PublisherLabel.Font.Color := clWhite;
  PublisherLabel.Left := 30;
  PublisherLabel.Top := 22;
  PublisherLabel.Width := TopPanel.Width - 60;

  // Create App Name & Version Label
  AppNameLabel := TNewStaticText.Create(WizardForm);
  AppNameLabel.Parent := TopPanel;
  AppNameLabel.Caption := '{#MyAppName} {#MyAppVersion}';
  AppNameLabel.Font.Name := 'Segoe UI';
  AppNameLabel.Font.Size := 12;
  AppNameLabel.Font.Color := clWhite;
  AppNameLabel.Left := 30;
  AppNameLabel.Top := 65;
  AppNameLabel.Width := TopPanel.Width - 60;

  // Create Middle Body Panel
  MiddlePanel := TPanel.Create(WizardForm);
  MiddlePanel.Parent := WizardForm;
  MiddlePanel.Left := 0;
  MiddlePanel.Top := 120;
  MiddlePanel.Width := WizardForm.ClientWidth;
  MiddlePanel.Height := 270;
  MiddlePanel.BevelOuter := bvNone;
  MiddlePanel.Color := clWhite;

  // Create Custom Install Button
  InstallButton := TButton.Create(WizardForm);
  InstallButton.Parent := MiddlePanel;
  InstallButton.Caption := 'Install Now';
  InstallButton.Left := 235;
  InstallButton.Top := 110;
  InstallButton.Width := 200;
  InstallButton.Height := 45;
  InstallButton.Font.Size := 11;
  InstallButton.Font.Style := [fsBold];
  InstallButton.Cursor := crHand;
  InstallButton.ElevationRequired := True;
  InstallButton.OnClick := @InstallButtonClick;

  // Reparent progress controls to MiddlePanel
  WizardForm.StatusLabel.Parent := MiddlePanel;
  WizardForm.StatusLabel.Left := 50;
  WizardForm.StatusLabel.Top := 90;
  WizardForm.StatusLabel.Width := 570;
  WizardForm.StatusLabel.Font.Color := clBlack;
  WizardForm.StatusLabel.Visible := False;
  
  WizardForm.ProgressGauge.Parent := MiddlePanel;
  WizardForm.ProgressGauge.Left := 50;
  WizardForm.ProgressGauge.Top := 120;
  WizardForm.ProgressGauge.Width := 570;
  WizardForm.ProgressGauge.Height := 25;
  WizardForm.ProgressGauge.Visible := False;
  
  WizardForm.FilenameLabel.Parent := MiddlePanel;
  WizardForm.FilenameLabel.Left := 50;
  WizardForm.FilenameLabel.Top := 155;
  WizardForm.FilenameLabel.Width := 570;
  WizardForm.FilenameLabel.Font.Color := clGray;
  WizardForm.FilenameLabel.Visible := False;

  // Create Success Label (hidden initially)
  SuccessLabel := TNewStaticText.Create(WizardForm);
  SuccessLabel.Parent := MiddlePanel;
  SuccessLabel.Caption := 'RUKN Search has been installed successfully!';
  SuccessLabel.Font.Size := 13;
  SuccessLabel.Font.Style := [fsBold];
  SuccessLabel.Font.Color := clBlack;
  SuccessLabel.Left := 50;
  SuccessLabel.Top := 110;
  SuccessLabel.Width := 570;
  SuccessLabel.Alignment := taCenter;
  SuccessLabel.Visible := False;

  // Create Bottom Footer Panel
  BottomPanel := TPanel.Create(WizardForm);
  BottomPanel.Parent := WizardForm;
  BottomPanel.Left := 0;
  BottomPanel.Top := 390;
  BottomPanel.Width := WizardForm.ClientWidth;
  BottomPanel.Height := 60;
  BottomPanel.BevelOuter := bvNone;
  BottomPanel.Color := $F0F0F0;

  // Create Terms and Conditions Button
  TermsButton := TButton.Create(WizardForm);
  TermsButton.Parent := BottomPanel;
  TermsButton.Caption := 'View Store Terms and Conditions';
  TermsButton.Left := 20;
  TermsButton.Top := 15;
  TermsButton.Width := 240;
  TermsButton.Height := 30;
  TermsButton.Cursor := crHand;
  TermsButton.OnClick := @TermsButtonClick;

  // Create Custom Cancel Button
  CancelBtn := TButton.Create(WizardForm);
  CancelBtn.Parent := BottomPanel;
  CancelBtn.Caption := 'Cancel';
  CancelBtn.Left := 530;
  CancelBtn.Top := 15;
  CancelBtn.Width := 120;
  CancelBtn.Height := 30;
  CancelBtn.Cursor := crHand;
  CancelBtn.OnClick := @CustomCancelButtonClick;
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  if PageID = wpReady then
    Result := True
  else
    Result := False;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpWelcome then
  begin
    InstallButton.Visible := True;
    WizardForm.StatusLabel.Visible := False;
    WizardForm.ProgressGauge.Visible := False;
    WizardForm.FilenameLabel.Visible := False;
    SuccessLabel.Visible := False;
  end
  else if CurPageID = wpInstalling then
  begin
    InstallButton.Visible := False;
    WizardForm.StatusLabel.Visible := True;
    WizardForm.ProgressGauge.Visible := True;
    WizardForm.FilenameLabel.Visible := True;
    SuccessLabel.Visible := False;
  end
  else if CurPageID = wpFinished then
  begin
    InstallButton.Visible := False;
    WizardForm.StatusLabel.Visible := False;
    WizardForm.ProgressGauge.Visible := False;
    WizardForm.FilenameLabel.Visible := False;
    SuccessLabel.Visible := True;
    CancelBtn.Caption := 'Finish';
  end;
end;
