rd /q /s Release
md Release
cd Release

svn export https://cslacontrib.svn.codeplex.com/svn/trunk/samples CslaContribSamples

svn export https://cslacontrib.svn.codeplex.com/svn/trunk/License.txt
svn export https://cslacontrib.svn.codeplex.com/svn/trunk/Readme.txt

del /q ..\CslaContribSamples.zip
"C:\Program Files\7-Zip\7z" a -r -tzip ..\CslaContribSamples *

cd ..
rd /q/s Release
