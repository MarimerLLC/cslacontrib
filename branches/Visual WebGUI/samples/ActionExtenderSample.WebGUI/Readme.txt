+-------------------------------------------+
| Read me file for ActionExtenderSample.sln |
+-------------------------------------------+

Target
------
This sample targets Csla 4.5.601, CslaContrib 4.5.600 and NET 4.5

Database engine
---------------
The database file is attached to LocalDb - the new dedicated version of 
SQL Express for developers. If you aren't running VS2012 chances are 
you need to download it at
http://www.microsoft.com/en-us/download/details.aspx?id=29062

Look for SqlLocalDB.MSI (x86 or x64).

Connection string
-----------------
In order to have a path independent connection string, the database file is 
copied to the build folder. The side efect is that any changes you made to 
database will go away when you clean the solution or the project.

Visual WebGUI
-------------
The project file and web.config were created using
Visual WebGUI Professional Studio 7.1.3 (May 11, 2014)

Solution doesn't load?
======================
Go to http://www.visualwebgui.com/Developers/Downloads/tabid/110/Default.aspx
Register yourself and login. Now download:
    - Professional Studio 7.1.3 (.NET 4.5)
