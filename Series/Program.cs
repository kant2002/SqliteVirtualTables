using Microsoft.Data.Sqlite;
using SqliteVirtualTables;

using (var connection = new SqliteConnection("Data Source=:memory:"))
{
    connection.Open();

    using var module = new SeriesModule(connection);

    var command = connection.CreateCommand();
    command.CommandText =
    @"
        SELECT * FROM generate_series(0,100,5);
    ";
    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            var name = reader.GetInt64(0);

            Console.WriteLine($"Hello, {name}!");
        }
    }
}