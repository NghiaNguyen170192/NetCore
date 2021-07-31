# NetCore
Dotnet Core Project

To add migrations:
	1/ Open Powershell and navigate to NetCore.Infrastructure.Database
	2/ run "dotnet ef migrations add your_migration_name"

To Run:
	1/ Navigate to \docker, run "docker-compose up -d" to start database
	2/ Build the NetCore.sln
	3/ Edit Connectionstring with your connectionstring in these projects:
		NetCore.Tools.Migrations
		NetCore.Api
	4/ Right click on NetCore.sln, click Properties, set Multiple Startup projects:
		NetCore.Api
		
