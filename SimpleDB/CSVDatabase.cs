namespace SimpleDB;

using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public sealed class CSVDatabase<T> : IDatabaseRepository<T> {
    public string fileName { get; set; } = null!;
    public IEnumerable<T> Read(int? limit = null) {
        var Sr = new StreamReader(fileName);
        var Csv = new CsvReader(Sr, CultureInfo.InvariantCulture);
        IEnumerable<T> Records = Csv.GetRecords<T>();
        return Records;
    }
    public void Store(T record) {
        // Next couple of lines are inspired by https://joshclose.github.io/CsvHelper/examples/writing/appending-to-an-existing-file/
        // Append to the file.
        var Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
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