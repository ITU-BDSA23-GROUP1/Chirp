namespace SimpleDB;

using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    // The following is inspired from:
    // https://csharpindepth.com/Articles/Singleton
    private static readonly Lazy<CSVDatabase<T>> lazy =
        new Lazy<CSVDatabase<T>>(() => new CSVDatabase<T>());

    public static CSVDatabase<T> Instance { get { return lazy.Value; } }

    private CSVDatabase()
    {
    }

    public string fileName { get; set; } = null!;
    public IEnumerable<T> Read(int? limit = null)
    {
        // Reading the CSV file is inspired by:
        // - https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
        // - https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
        using (var Sr = new StreamReader(fileName))
        using (var Csv = new CsvReader(Sr, CultureInfo.InvariantCulture))
        {   
            foreach (var record in Csv.GetRecords<T>())
            {
                yield return record;
            }
        }     
    }
    public void Store(T record)
    {
        // Next couple of lines are inspired by https://joshclose.github.io/CsvHelper/examples/writing/appending-to-an-existing-file/
        // Append to the file.
        var Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header.
            HasHeaderRecord = false,
            // Don't make quotation marks automatically
            // Next line is inspired by https://stackoverflow.com/questions/62460380/adding-double-quotes-to-string-while-writing-to-csv-c-sharp
            ShouldQuote = (context) => false
        };

        using (var Stream = File.Open(fileName, FileMode.Append))
        using (var Writer = new StreamWriter(Stream))
        using (var Csv = new CsvWriter(Writer, Config))
        {
            Csv.NextRecord();
            Csv.WriteRecord(record);
        }
    }
}