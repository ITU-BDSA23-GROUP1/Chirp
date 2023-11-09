namespace Chirp.Core;

public interface ICheepRepository<T, in Tstring>
{
    public Task<IEnumerable<T>> Get(int offset);
    public Task<IEnumerable<T>> GetByFilter(string attribute, int offset);
   
    public Task CreateCheep(CheepDTO cheepDTO);
}