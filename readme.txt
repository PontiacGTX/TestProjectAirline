INSTRUCTIONS
==================================================================


Download ASP.NET 5.02/5.04 Hosting Bundle
On the first time executing the project be sure run on the Entity Framework Core Tools 
Update-Database

if it doesnt create the database
Delete content in the folder migration on DataAccess/Migrations
Add-Migration InitialMigration
Update-Database

on the route add /swagger/index.html
then on the GetFlights API End point add a value in the payload between 0 and 2 and the route you want to check

on unit test the uri should be set to the url of the project