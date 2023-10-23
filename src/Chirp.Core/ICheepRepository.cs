namespace Chirp.Core;

public interface ICheepRepository<T, in Tstring>
{
    public Task<IEnumerable<T>> Get(int offset);
    public Task<IEnumerable<T>> GetByFilter(string attribute, int offset);
    public Task<AuthorDTO> FindAuthorByName(string name);
    public Task<AuthorDTO> FindAuthorByEmail(string email);
    public void CreateAuthor(int authorID, string name, string email); 
    public void CreateCheep(string text, DateTime timeStamp, AuthorDTO author, int authorID, int cheepID);
}