using Chirp.SQLite;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int offset, int limit);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    public List<CheepViewModel> GetCheeps(int offset, int limit)
    {
        //return _cheeps;

        List<CheepViewModel> cheeps = new();

        foreach (DBFacade.CheepDataModel cdm in DBFacade.GetDBCheeps(offset, limit))
        {
            cheeps.Add(new CheepViewModel(cdm.Author, cdm.Message, UnixTimeStampToDateTimeString(Convert.ToDouble(cdm.Timestamp))));
        }

        return cheeps;

    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return GetCheeps(0, 32).Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
