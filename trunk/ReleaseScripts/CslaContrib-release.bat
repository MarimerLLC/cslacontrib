rd /q /s Release
md Release
cd Release

xcopy /E ..\..\bin\Release

svn export https://cslacontrib.svn.codeplex.com/svn/trunk/License.txt
svn export https://cslacontrib.svn.codeplex.com/svn/trunk/Readme.txt

del /q ..\CslaContrib.zip
"C:\Program Files\7-Zip\7z" a -r -tzip ..\CslaContrib *

cd ..
rd /q/s Release
