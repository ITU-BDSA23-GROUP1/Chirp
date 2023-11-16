namespace Chirp.Core;

public interface IAuthorRepository<T, in Tstring>
{
    public Task<AuthorDTO> FindAuthorByName(string name);
    public Task<AuthorDTO> FindAuthorByEmail(string email);
    public void CreateAuthor(AuthorDTO authorDTO);
    public Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO);
    public Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO);
    public Task<IEnumerable<string>> GetFollowing(AuthorDTO authorDTO);

}