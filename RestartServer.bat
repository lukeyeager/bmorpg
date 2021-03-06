ECHO	off

REM		RestartServer.bat
REM		Feb 21, 2012
REM		TAMU, CSCE 431
REM		Luke Yeager

SET		serverDir=Server\bin
SET		releaseDir=%CD%\%serverDir%\Release
SET		debugDir=%CD%\%serverDir%\Debug
SET		serverExe=BMORPG_SERVER.exe
SET		serverPort=""

:ParameterLoop
IF		"%1"=="" GOTO ParameterLoopEnd

REM		Check for SVN parameter
IF		"%1"=="svn" (
	ECHO	Updating the SVN at %CD%...
	TortoiseProc /command:update /path:"%CD%" /closeonend:1 
)
SHIFT
GOTO	ParameterLoop
:ParameterLoopEnd
	
ECHO	Restarting the server...

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
