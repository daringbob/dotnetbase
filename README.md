## THÀNH VIÊN
1. Lê Văn Tâm - 3120560086
2. Châu Quốc Thanh - 3120560089
## TASKS
- Tâm
	- Set up Generic Odata Controller
	- Send mail Service
	- Set up ORM -> Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.SqlServer
- Thanh
	- Set up Generic Bulk Controller
	- Lưu file -> File Controller, Firebase Storage Service
	- Chat realtime services
	- Authentication -> JWT
## Steps to run the project:
1.  Move to  **src**  folder.
2.  Run command-line  `dotnet restore`  to install all paackage enter code hereckages were used in project.
3. Run command-line `dotnet ef migrations add init-01` to create first migrations
4.  Run command-line  `dotnet ef database update`  in first time when run the project to create new database.
5. Run command-line `ipconfig` to get IPv4 then access `launchSettings.json`  change `http :{applicationUrl : http://Ipv4:5000}` and save
6.  Run command-line  `dotnet run`  to start server.
7.  Access  [**Swagger**](http://localhost:5000/swagger/index.html)  to run API.

## Requirements:

1.  Install  [**.NET SDK 8.0**](https://dotnet.microsoft.com/download).
2.  Install  **SQL Server**.
3.  Install  **dotnet ef**  in by command-line  `dotnet tool install --global dotnet-ef`. If  **dotnet ef**  was installed then using the following command line  `dotnet tool update --global dotnet-ef`  to update dotnet-ef tool.

## Extensions
- C# Dev kit
- .Net Extension Pack
- Nuget Gallery
- CSharpier - Code formatter
- C# Extension (JosKreativ)
