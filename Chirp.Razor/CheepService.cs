using Chirp.SQLite;

public record CheepViewModel(string Author, string Message, string Timestamp, string AuthorID);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int offset);
    public List<CheepViewModel> GetCheepsFromAuthor(int offset, string author);
}

public class CheepService : ICheepService
{
    public List<CheepViewModel> GetCheeps(int offset)
    {
        //return _cheeps;

        List<CheepViewModel> cheeps = new();

        foreach (DBFacade.CheepDataModel cdm in DBFacade.GetAllDBCheeps(offset))
        {
            cheeps.Add(new CheepViewModel(cdm.Author, cdm.Message, UnixTimeStampToDateTimeString(Convert.ToDouble(cdm.Timestamp)), cdm.UserID));
        }

        return cheeps;

    }

    public List<CheepViewModel> GetCheepsFromAuthor(int offset, string author)
    {
        // filter by the provided author name

        List<CheepViewModel> cheeps = new();

        foreach (DBFacade.CheepDataModel cdm in DBFacade.GetAuthorDBCheeps(offset, author))
        {
            cheeps.Add(new CheepViewModel(cdm.Author, cdm.Message, UnixTimeStampToDateTimeString(Convert.ToDouble(cdm.Timestamp)), cdm.UserID));
        }

        return cheeps;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
