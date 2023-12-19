namespace Chirp.Core;

/// <summary>
/// This interface represents a repository for Cheeps.
/// It declares different methods for accessing Cheeps in the database.
/// </summary>
public interface ICheepRepository
{
    public Task<IEnumerable<CheepDTO>> Get(int offset);
    public Task<IEnumerable<CheepDTO>> GetByFilter(string attribute, int offset);
    public Task<IEnumerable<CheepDTO>> GetByFollowers(List<string> authorNames, int offset);
    public Task CreateCheep(CheepDTO cheepDTO);
    public Task<bool> DeleteCheep(string cheepId);
}