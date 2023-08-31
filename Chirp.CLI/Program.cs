using System;
using System.IO;
using System.Text.RegularExpressions;

try
{
    // Open the text file using a stream reader.
    // Lines 10 - 20 inspired by https://stackoverflow.com/questions/3507498/reading-csv-files-using-c-sharp/34265869#34265869
    // Lines 10 - 23 inspired by https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-read-text-from-a-file
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
            DateTimeOffset localTime = DateTimeOffset.Now;
            var time = localTime.ToString(cheep[2]);
            Console.WriteLine($"{cheep[0]} @ {time}: {cheep[1].Substring(1, cheep[1].Length - 2)}");

        }
        // Read the stream as a string, and write the string to the console.
        //Console.WriteLine(sr.ReadToEnd());
    }
}
catch (IOException e)
{
    Console.WriteLine("The file could not be read:");
    Console.WriteLine(e.Message);
}
