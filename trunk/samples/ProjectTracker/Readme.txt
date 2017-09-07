Wisej
-----
Wisej is a de facto replacement for Visual WebGUI.
Take your WinForms project, port it to Wisej retaining all your BO/DAL code and most UI code.
Now run it as a Web application.
Get Wisej at http://wisej.com

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
