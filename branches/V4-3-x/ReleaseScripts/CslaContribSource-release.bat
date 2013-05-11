REM 2013-04-26 changed "svn export" to "XCOPY" as Codeplex server is too slow

rd /q /s Release
md Release
cd Release

xcopy /E /I ..\..\Source CslaContribSource
rem svn export https://cslacontrib.svn.codeplex.com/svn/trunk/Source CslaContribSource

copy ..\..\License.txt
copy ..\..\Readme.txt
rem svn export https://cslacontrib.svn.codeplex.com/svn/trunk/License.txt
rem svn export https://cslacontrib.svn.codeplex.com/svn/trunk/Readme.txt

del /q ..\CslaContribSource-4.3.16.zip
"C:\Program Files\7-Zip\7z" a -r -tzip ..\CslaContribSource-4.3.16.zip *

cd ..
rd /q/s Release
rd /q/s Release
