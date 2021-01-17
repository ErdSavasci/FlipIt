:: This script is run after successful build.

@echo off

rem powershell -Command "Start-Process cmd -ArgumentList('/K', 'C:\Windows\System32\xcopy.exe /Y \"E:\FlipIt-master\src\FlipIt\bin\Release\FlipIt.exe\" \"C:\Windows\System32\FlipIt.scr\"')" -Verb RunAs
rem powershell -Command "Start-Process cmd -ArgumentList('/K', 'C:\Windows\System32\xcopy.exe /Y \"E:\FlipIt-master\src\FlipIt\bin\Release\FlipIt.exe\" \"C:\Windows\SysWOW64\FlipIt.scr\"')" -Verb RunAs

:: Below lines use 'cp' command because of directory redirection of x64 systems. (System32 -> SysWOW64)
:: Also even the x64 systems do not need screensavers to be copied into System32 folder, it is done by this script intentionally and for experimental purpose.
:: So, the System32 lines can be safely deleted.

cp %cd%\FlipIt.exe C:\Windows\System32\FlipIt.scr
cp %cd%\FlipIt.exe C:\Windows\SysWOW64\FlipIt.scr