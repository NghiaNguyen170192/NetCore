# NetCore
Dotnet Core Project

To add migrations:
	1/ Open Powershell and navigate to NetCore.Infrastructure.Database
	2/ run "dotnet ef migrations add your_migration_name"

To Run:
	1/ Build the NetCore.sln
	2/ Edit Connectionstring with your connectionstring in these projects:
		NetCore.Tools.Migrations
		NetCore.Api
	3/ Right click on NetCore.sln, click Properties, set Multiple Startup projects:
		NetCore.Api
		
