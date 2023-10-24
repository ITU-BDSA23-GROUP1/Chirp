namespace Chirp.Core;

public interface IAuthorRepository<T, in Tstring>
{
    public Task<AuthorDTO> FindAuthorByName(string name);
    public Task<AuthorDTO> FindAuthorByEmail(string email);
    public void CreateAuthor(AuthorDTO authorDTO);

}