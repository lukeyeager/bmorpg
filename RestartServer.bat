echo off

REM		RestartServer.bat
REM		Feb 21, 2012
REM		TAMU, CSCE 431
REM		Luke Yeager

SET		serverDir=Server\bin
SET		releaseDir=%CD%\%serverDir%\Release
SET		debugDir=%CD%\%serverDir%\Debug
SET		serverExe=BMORPG_SERVER.exe
SET		serverPort=10000

echo	Updating the SVN...
TortoiseProc /command:update /path:"%CD%" /closeonend:1

echo	Restarting the server...

REM		NOTE: This script uses the Release version of the executable if it can
IF EXIST	%releaseDir%\%serverExe% (
	CD		%releaseDir%
	FOR %%? IN (%serverExe%) DO (
		ECHO	Using executable: %releaseDir%\%serverExe%
		ECHO	Last modified: %%~t?
	)
	START	/B  %serverExe% %serverPort%
) ELSE (
	REM		If the release executable is not available, it uses the Debug executable
	IF EXIST	%debugDir%\%serverExe% (
		CD		%debugDir%
		FOR %%? IN (%serverExe%) DO (
			ECHO	Using executable: %debugDir%\%serverExe%
			ECHO	Last modified: %%~t?
		)
		START	/B %serverExe% %serverPort%
	) ELSE (
		ECHO	ERROR: Could not find %serverExe%!
	)
)

EXIT
