@echo off
setlocal enabledelayedexpansion

:: Set the output file name
set outputFile=AudioClipNames.cs

:: Start writing to the output file
echo using System; > %outputFile%
echo public enum AudioClipName >> %outputFile%
echo { >> %outputFile%

:: .ogg files
for %%f in (*.ogg) do (
    :: Remove the file extension
    set fileName=%%~nf
    :: Replace spaces with underscores
    set fileName=!fileName: =_!
    :: Write the enum entry to the output file
    echo     !fileName!, >> %outputFile%
)
for %%f in (*.wav) do (
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