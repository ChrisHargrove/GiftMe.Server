# Database Information
## Connection Strings
When trying to connect Npqsl to the database it does not accept connections strings in the following format:
</br>`postgresql://<host>:<port>/<db_name>?user=<username>&password=<password>`
</br></br> The ef designer will complain and wont allow the service to connect to the db. To make it work
it needs to be in the following format.
</br>`Server=<hostname>;Port=<port>;User Id=<username>;Password=<password>;Database=<db_name>;`
</br> Once changed to the second format all the "dotnet ef" command will work and the service will
correctly connect to the DB.

## Commands
### Create Migration
When creating a database make sure that your are in the solution directory not a subproject and then
you can execute the following command to create a new migration.</br>
Make sure that you change name parameter to what you want the migration to be called.

```bash
dotnet ef migrations add <name> --project DataLayer --startup-project Api
```

### Remove Last Migration
When you need to modify the last created migration you can run the following command which will remove the last migration
and put the model snapshot to the last known configuration.
```bash
dotnet ef migrations remove --project DataLayer --startup-project Api
```

### Apply migrations
When applying the migrations to the database make sure you are in the top level of the solution before execution.
Then you can execute the following command to bring the database up to date.
```bash
dotnet ef database update --project DataLayer --startup-project Api   
```

### Undo last applied migration
Sometimes you will need to remove the last migration that was applied to the database. This can be achieved
using the following command.
```bash
dotnet ef database update <name_of_migration> --project DataLayer --startup-project Api
```