@echo off
where cl.exe
if not %ERRORLEVEL%==0 (
	echo Cannot find cl.exe
	goto :EOF
)
cl /P /C src\DecryptionV2_Unpreprocessed.cs
move /Y DecryptionV2_Unpreprocessed.i  src\DecryptionV2.cs
cl /P /C src\App_Unpreprocessed.cs
move /Y App_Unpreprocessed.i  src\App.cs
