# Board games API application

This application is simple catalog of diferent board games which stored in database and can be utilized using application REST API (CRUD).

To run this application you should perform actions listed below.

## 1. Data base settings

- If not installed, install `MS SQL` server and `MS SSMS`  
- Create data base whih will be used this application to store their data:  
    - Press MRB (mouse right button) on section `Базы данных` and select `Создать базу данных...`  
    - Then enter database name in field `Имя базы данных:` of dialog window `Создание базы данных` (for example it might be `MyBGList`). Aafter then click `OK`.  
- Create user which will be mediaton in interaction this application and their data base:
    - Pres MBR on section `Безопасность` (of SQL server, not inside database) and then select `Создать` → `Вход...`
    - In floating dialog windows select page `Общие` and set next parameters:
      - `Имя для входа:` (for example it might be `MyBGList`)
      - Select `Проверка подлинности SQL Server`
      - `Пароль:` and `Подтверждение пароля:` (for example it might be `MyS3cretP4$$`)
      - From dropdown list `База данных по умолчанию:` select name of previously created DB (in example it was `MyBGList`)
    - In floating dialog windows select page `Сопоставление пользователей` and set next parameters:
      - Ensute in section `Пользователи, сопоставленніе с єтим именем для входа:` selected previousle created DB (in example it was `MyBGList`)
      - In section `Членство в роли базы данных для: <Name of DB>` select two roles `db_owner` and `public`
    - press OK in bottom of dialog window
- Check sequriti settings of SQL sever.
    - Press BMR on root section of SQL server and select `Свойства`
    - Select page `Безопасность` and ensure that in section `Серверная проверка подлинности` selected `Проверка подлинности SQL Server и Windows`, if not - check it.
    - In case if you change this parameter you should reboot your MS SQL server!

## 2. Set up DB connection string
- After you open application in Visual Studio press MBR direct on `MyBGList` project and select `Manage User Secrets`
- Insert your connection string info, in this maner:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyBGList; User Id=MyBGList;Password=MyS3cretP4$$; Integrated Security=False;MultipleActiveResultSets=True; TrustServerCertificate=True"
  }
}
```
- If you used other values for DB name, user name or password make apropriated changes
- Save your changes

## 3. Perform DB migrations
- In this application installed package `Microsoft.EntityFrameworkCore.Tool` which allow you perfor you update database state according to current application datamodel
- To perform this you should open menu `View` → `Other Windows` → `Package Manager Console` (PM)
- In PM print `Update-Database` - Structure of application DB will be updated and now application redy to start.

## 4. Seeding application DB
- In Visual Studio select `MyBGList (Development)` application profile and run application
- After application start you should to see Swagger documentation page
- Scroll down to the section `Seed` and press right handle of `PUT /Seed` action method
- Then press `Try it out` button, and then `Execute` button. Application database will be filled with data from file `Data\bgg_dataset.csv`.
- Now application ready! You can use other action methods from Swagger page.


## [Other helpfull Migration commands (PM)](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/managing?tabs=vs)
| Action | Command sample |
|---|---|
| Add a migration | Add-Migration `MigrationName` |
| Remove migration | Remove-Migration |
| Listing migrations | Get-Migration |
| Apply migrations | Update-Database |
| Rollback to ... | Update-Database `MigrationName` |
| Rollback to empty DB | Update-Database 0 |