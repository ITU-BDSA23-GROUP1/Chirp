namespace Chirp.Core;

public interface IAuthorRepository<T, in Tstring>
{
    public Task<AuthorDTO> FindAuthorByName(string name);
    public Task<AuthorDTO> FindAuthorByEmail(string email);
    public void CreateAuthor(AuthorDTO authorDTO);
    public void FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO);
    public void UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO);
    public IEnumerable<AuthorDTO> GetFollowing(AuthorDTO authorDTO);

}