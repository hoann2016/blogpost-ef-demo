Run SQL Server docker
``` 
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin@123"    -p 1438:1433 --name sql-ef-demo-blog-post --hostname sql-ef-demo-blog-post   -d    mcr.microsoft.com/mssql/server:2022-latest
```