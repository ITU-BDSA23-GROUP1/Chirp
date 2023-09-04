using System;
using System.IO;
using System.Text.RegularExpressions;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

 //Reading the CSV file is inspired by:
 // - https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
// - https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file

// Open the text file using a stream reader.

try{
    if (args[0] == "read") {
        using (var sr = new StreamReader("chirp_cli_db.csv"))
        {
            string line; 
            sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                //Define pattern
                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                //Separating columns to array
                string[] cheep = CSVParser.Split(line);
                // Following lines inspired by https://peterdaugaardrasmussen.com/2022/11/26/csharp-convert-datetimeoffset-to-and-from-unix-timestamp/#:~:text=In%20order%20to%20get%20a%20DateTime%20form%20a,DateTimeOffset.FromUnixTimeMilliseconds%281669321628392%29%3B%20var%20dateTime%20%3D%20dateTimeOffset.DateTime%3B%20That%20is%20all%21
                DateTimeOffset time = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cheep[2]));
                // Following lines inspired by  https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tolocaltime?view=net-7.0
                var localTime = time.DateTime.ToLocalTime();
                Console.WriteLine($"{cheep[0]} @ {localTime.ToString("MM/dd/yy HH:mm:ss")}: {cheep[1].Substring(1, cheep[1].Length - 2)}");
            }
        }

} else if (args[0] == "cheep") {
        // Next couple of lines are inspired by https://joshclose.github.io/CsvHelper/examples/writing/appending-to-an-existing-file/
        // Append to the file.
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        // Don't write the header again.
        HasHeaderRecord = false,
    };
    using (var stream = File.Open("chirp_cli_db.csv", FileMode.Append))
    using (var writer = new StreamWriter(stream))
    using (var csv = new CsvWriter(writer, config))
    {
        DateTime currentTime = DateTime.Now;
        long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

        csv.NextRecord();
        //Following line inspired by https://learn.microsoft.com/en-us/dotnet/api/system.environment.username?view=net-7.0
        csv.WriteRecord(new Cheep {author = Environment.UserName, message = '"' + args[1] + '"', timestamp = unixTime});
    }
} 
    }
    catch (IOException e)
    {
        Console.WriteLine("The file could not be read:");
        Console.WriteLine(e.Message);
    }

public class Cheep {
    public string author { get; set; } = null!;
    public string message { get; set; } = null!;
    public long timestamp { get; set; }
}
