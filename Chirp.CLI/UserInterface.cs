namespace Chirp.CLI;

public class UserInterface {
    public static void PrintCheeps(IEnumerable<Cheep> Cheeps) {
        foreach (Cheep CurCheep in Cheeps)
        {
            // Following lines inspired by https://peterdaugaardrasmussen.com/2022/11/26/csharp-convert-datetimeoffset-to-and-from-unix-timestamp/#:~:text=In%20order%20to%20get%20a%20DateTime%20form%20a,DateTimeOffset.FromUnixTimeMilliseconds%281669321628392%29%3B%20var%20dateTime%20%3D%20dateTimeOffset.DateTime%3B%20That%20is%20all%21
            DateTimeOffset Time = DateTimeOffset.FromUnixTimeSeconds(CurCheep.Timestamp);
            // Following lines inspired by  https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tolocaltime?view=net-7.0
            var LocalTime = Time.DateTime.ToLocalTime();
            Console.WriteLine($"{CurCheep.Author} @ {LocalTime}: {CurCheep.Message}");
        }
    }
}