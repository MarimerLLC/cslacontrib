rd /q /s Release
md Release
cd Release

xcopy /E ..\..\bin\Release

svn export https://cslacontrib.svn.codeplex.com/svn/trunk/License.txt
svn export https://cslacontrib.svn.codeplex.com/svn/trunk/Readme.txt

"C:\Program Files\7-Zip\7z" a -r -tzip ..\CslaContribSource *

cd ..
rd /q/s Release
