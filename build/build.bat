@ECHO OFF

SET TOOLS_DIR=tools\

CALL :checkIfRunningFromInvalidPath RESULT
IF %RESULT% == 1 (
	ECHO Please run this script from the root project folder.
	GOTO :eof
)

SET FAKE=%TOOLS_DIR%FAKE\tools\Fake.exe
IF NOT EXIST %FAKE% (
    ECHO FAKE not found. Installing...
	
	PUSHD %TOOLS_DIR%
	nuget\nuget.exe install FAKE -ExcludeVersion -NonInteractive
	POPD
)

SET NUNIT3=%TOOLS_DIR%NUnit.ConsoleRunner\tools\nunit3-console.exe
IF NOT EXIST %NUNIT3% (
    ECHO NUNIT3 not found. Installing...
	
	PUSHD %TOOLS_DIR%
	nuget\nuget.exe install NUnit.ConsoleRunner -ExcludeVersion -NonInteractive
	POPD
)

%FAKE% %* --nocache
GOTO :eof

:checkIfRunningFromInvalidPath
SET "%~1=1"
SET WD1="%cd%"
PUSHD "%~dp0"
CD ..
SET WD2="%cd%"
POPD
IF %WD1% == %WD2% (
	SET "%~1=0"
)

GOTO :eof