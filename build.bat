@ echo off

del imgTI.exe

call C:\Windows\Microsoft.net\framework64\v3.5\csc.exe imgTI.cs

call imgTI.exe img.png

pause