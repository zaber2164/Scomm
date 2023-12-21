# Scomm Test
 Steps to run the code:
 
1.	First change the DefaultConnection in the appsettings.json
2.	Run the following commands in succession in package manager console:-
Remove-Migration -Force,
Add-Migration InitialMigration,
Update-Database
3.	Set the Scomm project as startup & run using F5
4.	Login as “Admin” using the credentials:-
Email = "admin@admin.com"; password = "Test@123"
5.	Browse 

# some things I skipped due to lack of time:

1.	Bulk insert using stored procedure which could be done using sql server table type & EF/dapper
2.	Unit testing
3.	In memory caching using IMemoryCache

