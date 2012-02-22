REM	RestartServer.bat
REM	Feb 21, 2012
REM	TAMU, CSCE 431
REM	Luke Yeager

REM	Update from SVN
TortoiseProc /command:update /path:"%CD%" /closeonend:1

REM	Restart the server
CD	"Server\bin\Release"
START	/B BMORPG_SERVER.exe 10000

EXIT
