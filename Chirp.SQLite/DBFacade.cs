namespace Chirp.SQLite;

using System.Data;
using Microsoft.Data.Sqlite;

public class DBFacade
{

    public static void Main()
    {
        // This is to be able to run 'dotnet run' to test if our desired methods works as intended in the terminal
        foreach (CheepDataModel cdm in GetAllDBCheeps(0))
        {
            Console.WriteLine($"Author = {cdm.Author}, Message = {cdm.Message}");
        }
    }

    public record CheepDataModel(string Author, string Message, string Timestamp, string MessageID, string UserID, string Email);

    public static List<CheepDataModel> GetAllDBCheeps(int offset)
    {
        var sqlQuery = @$"
        SELECT * FROM message m
        JOIN user u ON u.user_id = m.author_id
        ORDER BY m.pub_date desc
        LIMIT 32 OFFSET {offset}
        ";

        return GetDBCheeps(sqlQuery);
    }

    public static List<CheepDataModel> GetAuthorDBCheeps(int offset, string author_id)
    {
        var sqlQuery = @$"
        SELECT * FROM message m
        JOIN user u ON u.user_id = m.author_id
        WHERE m.author_id = {author_id}
        ORDER BY m.pub_date desc
        LIMIT 32 OFFSET {offset}
        ";

        return GetDBCheeps(sqlQuery);
    }

    private static List<CheepDataModel> GetDBCheeps(string query)
    {
        var sqlDBFilePath = "/tmp/chirp.db";

        //Inspired by Helge's Chirp.SQLite project 
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = command.ExecuteReader();

            var cheepList = new List<CheepDataModel>();

            while (reader.Read())
            {
                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-7.0
                // for documentation on how to retrieve complete columns from query results
                Object[] values = new Object[reader.FieldCount];
                reader.GetValues(values);
                cheepList.Add(new CheepDataModel(Author: $"{values[5]}", Message: $"{values[2]}", Timestamp: $"{values[3]}", MessageID: $"{values[0]}", UserID: $"{values[1]}", Email: $"{values[6]}"));
            }
            return cheepList;
        }
    }
}