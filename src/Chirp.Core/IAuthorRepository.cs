namespace Chirp.Core;

/// <summary>
/// This interface represents a repository for Authors.
/// It declares different methods for accessing Authors in the database.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="Tstring"></typeparam>
public interface IAuthorRepository<T, in Tstring>
{
    public Task<AuthorDTO> FindAuthorByName(string name);
    public Task<AuthorDTO> FindAuthorByEmail(string email);
    public Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO);
    public Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO);
    public Task<IEnumerable<string>> GetFollowing(AuthorDTO authorDTO);
    public Task<bool> DeleteAuthor(string authorId);
    public Task<IEnumerable<string>> GetFollowingNames(AuthorDTO authorDTO);
}