namespace Chirp.Core;

/// <summary>
/// This interface represents a repository for Cheeps.
/// It declares different methods for accessing Cheeps in the database.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="Tstring"></typeparam>
public interface ICheepRepository<T, in Tstring>
{
    public Task<IEnumerable<T>> Get(int offset);
    public Task<IEnumerable<T>> GetByFilter(string attribute, int offset);
    public Task<IEnumerable<CheepDTO>> GetByFollowers(List<string> authorNames, int offset);
    public Task CreateCheep(CheepDTO cheepDTO);
    public Task<bool> DeleteCheep(string cheepId);
}