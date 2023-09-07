using System;
using System.IO;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

// Reading the CSV file is inspired by:
// - https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
// - https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file

// Open the text file using a stream reader.

try
{
    if (args[0] == "read")
    {
        Read();
    } else if (args[0] == "cheep") 
    {
        Write(args[1]);
    } 
}
catch (IOException E)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(E.Message);
}

static void Read() {
    using (var Sr = new StreamReader("chirp_cli_db.csv"))
    {
        string? Line; 
        Sr.ReadLine();
        while ((Line = Sr.ReadLine()) != null)
        {
            //Define pattern
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            //Separating columns to array
            string[] Cheep = CSVParser.Split(Line);
            // Following lines inspired by https://peterdaugaardrasmussen.com/2022/11/26/csharp-convert-datetimeoffset-to-and-from-unix-timestamp/#:~:text=In%20order%20to%20get%20a%20DateTime%20form%20a,DateTimeOffset.FromUnixTimeMilliseconds%281669321628392%29%3B%20var%20dateTime%20%3D%20dateTimeOffset.DateTime%3B%20That%20is%20all%21
            DateTimeOffset Time = DateTimeOffset.FromUnixTimeSeconds(long.Parse(Cheep[2]));
            // Following lines inspired by  https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tolocaltime?view=net-7.0
            var LocalTime = Time.DateTime.ToLocalTime();
            Console.WriteLine($"{Cheep[0]} @ {LocalTime.ToString("MM/dd/yy HH:mm:ss")}: {Cheep[1].Substring(1, Cheep[1].Length - 2)}");
        }
    }
}


static void Write(string CheepMsg) {
    // Next couple of lines are inspired by https://joshclose.github.io/CsvHelper/examples/writing/appending-to-an-existing-file/
        // Append to the file.
        var Config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            // Don't write the header again.
            HasHeaderRecord = false,
            // Following lines are inspired by https://stackoverflow.com/questions/56579848/csvhelper-configuration-shouldquote-return-true-for-only-fields-on-the-dto-whi
            ShouldQuote = args =>
            {
                if (string.IsNullOrEmpty(args.Field)) return false;
                else if (args.Field.Equals(Environment.UserName)) return false;

                return args.FieldType == typeof(string);
            }
        };
    
        using (var Stream = File.Open("chirp_cli_db.csv", FileMode.Append))
        using (var Writer = new StreamWriter(Stream))
        using (var Csv = new CsvWriter(Writer, Config))
        {
            DateTime CurrentTime = DateTime.Now;
            long UnixTime = ((DateTimeOffset)CurrentTime).ToUnixTimeSeconds();

            Csv.NextRecord();
            //Following line inspired by https://learn.microsoft.com/en-us/dotnet/api/system.environment.username?view=net-7.0
            Csv.WriteRecord(new Cheep {Author = Environment.UserName, Message = CheepMsg, Timestamp = UnixTime});
        }
}

public record Cheep {
    public string Author { get; set; } = null!;
    public string Message { get; set; } = null!;
    public long Timestamp { get; set; }
}
