using System;
using System.IO;
using System.Text.RegularExpressions;

try
{
    // Lines 10 - 20 inspired by https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
    // Lines 10 - 24 inspired by https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
    // Open the text file using a stream reader.
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
            // Lines 22-23 inspired by https://peterdaugaardrasmussen.com/2022/11/26/csharp-convert-datetimeoffset-to-and-from-unix-timestamp/#:~:text=In%20order%20to%20get%20a%20DateTime%20form%20a,DateTimeOffset.FromUnixTimeMilliseconds%281669321628392%29%3B%20var%20dateTime%20%3D%20dateTimeOffset.DateTime%3B%20That%20is%20all%21
            DateTimeOffset localTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(cheep[2]));
            var time = localTime.DateTime;
            Console.WriteLine($"{cheep[0]} @ {time}: {cheep[1].Substring(1, cheep[1].Length - 2)}");

        }
    }
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}
