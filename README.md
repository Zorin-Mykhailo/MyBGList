- Call context menu on project and select `Manage user secret`
- Insert your connection string info, in this maner:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyBGList; User Id=MyBGList;Password=MyS3cretP4$$; Integrated Security=False;MultipleActiveResultSets=True; TrustServerCertificate=True"
  }
}
```


- Install package `Microsoft.EntityFrameworkCore.Tools`
- Open `Package Manager Console` window
- Execute commands:

```
Add-Migration InitialCreate
```
 
```
Update-Database
```


# [Migration commands](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=vs)

Add a migration
```
Add-Migration AddBlogCreatedTimestamp
```

Namespaces
```
Add-Migration InitialCreate -OutputDir Your\Directory
```

Remove a migration
```
Remove-Migration
```

Listing migrations
```
Get-Migration
```

Apply migrations
```
Update-Database
```

Rollback to...
```
Update-Database MigrationName
```

Rollback to empty DB
```
Update-Database 0
```