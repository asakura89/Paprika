@echo off
cls
goto :init

:init
    echo.
    echo Initializing
    set hour=%time:~0,2%
    if "%hour:~0,1%" == " " set hour=0%hour:~1,1%
    set min=%time:~3,2%
    if "%min:~0,1%" == " " set min=0%min:~1,1%
    set secs=%time:~6,2%
    if "%secs:~0,1%" == " " set secs=0%secs:~1,1%

    set year=%date:~-4%
    set month=%date:~3,2%
    if "%month:~3,1%" == " " set month=0%month:~3,1%
    set day=%date:~0,2%
    if "%day:~0,1%" == " " set day=0%day:~1,1%

    set datetimef=%year%%month%%day%%hour%%min%%secs%

    echo Done
    echo.
    goto :do

:error
    echo.
    echo ---------------------------------------------------------------------
    echo Failed to publish.
    echo.
    goto :exit

:do
    echo Building
    dotnet publish -p:PublishSingleFile=true -r win-x64 -c Release --self-contained true -p:PublishTrimmed=true -p:EnableCompressionInSingleFile=true .\Paprika\Paprika.csproj
    if errorlevel 1 goto :build-error

:done
    echo.
    echo ---------------------------------------------------------------------
    echo Publish finished.
    echo.

:exit
