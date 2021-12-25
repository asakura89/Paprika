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

    set git="C:\Program Files\Git\bin\git.exe"
    set precleanflags=clean -x -d --dry-run
    set cleanflags=clean -x -d -f
    echo Done
    echo.
    goto :clean-workspace

:build-error
    echo.
    echo ---------------------------------------------------------------------
    echo Failed to compile.
    echo.
    goto :exit

:clean-workspace
    echo Cleanup workspace
    %git% %precleanflags%
    %git% %cleanflags%
    echo Done
    echo.
    goto :build

:build
    echo Building
    dotnet build .\Paprika\Paprika.csproj
    if errorlevel 1 goto :build-error

:done
    echo.
    echo ---------------------------------------------------------------------
    echo Compile finished.
    echo.

:exit
