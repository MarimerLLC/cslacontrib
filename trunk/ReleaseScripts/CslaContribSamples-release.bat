REM 2013-04-26 changed "svn export" to "XCOPY" as Codeplex server is too slow

rd /q /s Release
md Release
cd Release

xcopy /E /I ..\..\samples CslaContribSamples
rem svn export https://cslacontrib.svn.codeplex.com/svn/trunk/samples CslaContribSamples

copy ..\..\License.txt
copy ..\..\Readme.txt
rem svn export https://cslacontrib.svn.codeplex.com/svn/trunk/License.txt
rem svn export https://cslacontrib.svn.codeplex.com/svn/trunk/Readme.txt

del /q ..\CslaContribSamples-4.5.50.zip
"C:\Program Files\7-Zip\7z" a -r -tzip ..\CslaContribSamples-4.5.50.zip *

cd ..
rd /q/s Release
