; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

[Setup]
AppName=Rule
AppVersion=1.0
DefaultDirName={pf}\Rule
DefaultGroupName=Josh2112 Apps
UninstallDisplayIcon={app}\Rule.exe
Compression=lzma2
SolidCompression=yes
OutputDir=Installer

[Files]
Source: "bin\Release\Rule.exe"; DestDir: "{app}"

[Icons]
Name: "{group}\Rule"; Filename: "{app}\Rule.exe"
Name: "{group}\Uninstall Rule"; Filename: "{uninstallexe}"