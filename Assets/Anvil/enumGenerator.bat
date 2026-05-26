@echo off
setlocal enabledelayedexpansion

:: Set the output file name
set outputFile=AudioClipNames.cs

:: Start writing to the output file
echo using System; > %outputFile%
echo public enum AudioClipNames >> %outputFile%
echo { >> %outputFile%

:: Loop through all .ogg files in the current directory
for %%f in (*.ogg) do (
    :: Remove the file extension
    set fileName=%%~nf
    :: Replace spaces with underscores
    set fileName=!fileName: =_!
    :: Write the enum entry to the output file
    echo     !fileName!, >> %outputFile%
)


:: Close the enum definition
echo } >> %outputFile%

echo Enum file %outputFile% generated successfully.