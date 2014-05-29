// requires Windows 8.1 / 8 / 7 SP1 / Vista SP2 or Windows Server 2012 x64 / 2008 R2 SP1 x64 / 2008 SP2
// This is the web installer.

[CustomMessages]
dotnetfx45_title=.NET Framework 4.5

dotnetfx45_size=50 MB

;http://www.microsoft.com/globaldev/reference/lcid-all.mspx
en.dotnetfx45_lcid=''


[Code]
const
  dotnetfx45_url = 'http://download.microsoft.com/download/B/A/4/BA4A7E71-2906-4B2D-A0E1-80CF16844F5F/dotNetFx45_Full_setup.exe';

procedure dotnetfx45();
begin
	if (not netfxinstalled(NetFx45, '')) then
		AddProduct('dotNetFx45_Full_setup.exe',
			CustomMessage('dotnetfx45_lcid') + '/q /passive /norestart',
			CustomMessage('dotnetfx45_title'),
			CustomMessage('dotnetfx45_size'),
			dotnetfx45_url,
			false, false);
end;