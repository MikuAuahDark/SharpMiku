@echo off
rem This script allows to build SharpMiku with Visual Studio Command Prompt.
rem Usage: [call] MSVCBuild [target=Release] [cpu="Any CPU"]
rem Where: [target] can be Debug or Release (it's Release by default).
rem        [cpu] can be "Any CPU", "x86", or "x64"

if not defined VSINSTALLDIR goto VSMissing

call PreprocessV2Decrypt.bat
setlocal
if [%1] == [] (set BUILD_CONFIG=Release) else set BUILD_CONFIG=%1
if [%2] == [] (set BUILD_CPU=Any CPU) else set BUILD_CPU=%2
MSBuild SharpMiku.sln /p:Configuration=%BUILD_CONFIG% /p:Platform="%BUILD_CPU%" /m
endlocal
goto EOF

:VSMissing
echo Error: Make sure that you run this batch file from Visual Studio Command Prompt.
exit /b 1

:EOF
