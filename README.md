# SQLServerToQuickBase
VB.Net MVC API running under IIS and listening for requests from a formula URL button on a Quick Base form.
When it receives a request it queries SQL Server for a list of file paths.
Then it uploads those files to Quick Base using QuNect ODBC for QuickBase.
In SSMS, under the server, expand Security, then right click Logins and select "New Login...".
In the New Login dialog, enter "IIS APPPOOL" as the login name and click "OK".
Right click the new login, select Properties and select "User Mapping". 
Check the appropriate database, and the appropriate role, in this case db_datareader. 
You will also have to give "IIS APPPOOL" read permissions on the file system where the image files are located.
