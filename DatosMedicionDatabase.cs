using ControlPoso;
using SQLite;

public class DatosMedicionDatabase
{
    readonly SQLiteAsyncConnection _database;

    public DatosMedicionDatabase(string dbPath)
    {
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<DatosMedicion>().Wait();
    }

    public Task<int> SaveItemAsync(DatosMedicion item)
    {
        return _database.InsertAsync(item);
    }

    // ... otros métodos para manejar la base de datos ...

    public Task<List<DatosMedicion>> GetItemsAsync()
    {
        return _database.Table<DatosMedicion>().ToListAsync();
    }

}
