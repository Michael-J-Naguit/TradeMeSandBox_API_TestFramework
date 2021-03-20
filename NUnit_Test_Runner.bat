@echo off

cd C:\Workspace\TradeMeSandBox_API_TestFramework\packages\NUnit.ConsoleRunner.3.12.0\tools

nunit3-console.exe "C:\Workspace\TradeMeSandBox_API_TestFramework\bin\Debug\\TradeMeSandBox_API_TestFramework.dll" --workers=2

PAUSE