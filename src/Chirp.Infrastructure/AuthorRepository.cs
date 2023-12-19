namespace Chirp.Infrastructure;

/// <summary>
/// This class represents a repository for Authors.
/// It implements the IAuthorRepository interface.
/// </summary>
public class AuthorRepository : IAuthorRepository
{

    private readonly ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// This method finds an Author by the Author's username and returns the Author as an AuthorDTO.
    /// </summary>
    /// <param name="UserName">Username of the Author to be found</param>
    /// <returns>
    /// An AuthorDTO of the Author with the specified username.
    /// </returns>
    public async Task<AuthorDTO> FindAuthorByName(string UserName)
    {
        var author = await context.Authors
            .Where(a => a.UserName == UserName)
            .Select(a => new AuthorDTO
            {
                Id = a.Id,
                UserName = a.UserName,
                Email = a.Email
            })
            .FirstOrDefaultAsync(); //Maybe delete this line

        return author;
    }


    /// <summary>
    /// This method finds an Author by using the Author's DTO.
    /// It returns the Author, including the list of Authors that the Author is following.
    /// </summary>
    /// <param name="authorDTO">The AuthorDTO for which we wish to find the Author</param>
    /// <returns>
    /// The Author corresponding to the AuthorDTO. 
    /// </returns>
    public async Task<Author> FindAuthorByAuthorDTO(AuthorDTO authorDTO)
    {
        var author = await context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Id == authorDTO.Id);

        return author;
    }

    /// <summary>
    /// This method makes an Author follow another Author. 
    /// It takes two AuthorDTO's as parameters - the Author that wants to follow another Author 
    /// and the Author to be followed.
    /// The method adds the Author-to-be-followed to the list of Authors that the 
    /// Author is following. It also adds the Author to the list of Followers of the 
    /// Author-to-be-followed.
    /// </summary>
    /// <param name="authorDTO">The Author that wants to follow another Author</param>
    /// <param name="authorToFollowDTO">The Author to be followed</param>
    /// <returns></returns>
    public async Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO)
    {
        var author = await FindAuthorByAuthorDTO(authorDTO);
        var authorToFollow = await FindAuthorByAuthorDTO(authorToFollowDTO);

        author.Following.Add(authorToFollow);
        authorToFollow.Followers.Add(author);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// This method makes an Author unfollow another Author.
    /// It takes two AuthorDTO's as parameters - the Author that wants to unfollow another Author
    /// and the Author to be unfollowed.
    /// The method removes the Author-to-be-unfollowed from the list of Authors that the
    /// Author is following. It also removes the Author from the list of Followers of the
    /// Author-to-be-unfollowed.
    /// </summary>
    /// <param name="authorDTO">The Author that wants to unfollow another Author</param>
    /// <param name="authorToUnfollowDTO">The Author to be unfollowed</param>
    /// <returns></returns>
    public async Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO)
    {
        var author = await FindAuthorByAuthorDTO(authorDTO);
        var authorToUnfollow = await FindAuthorByAuthorDTO(authorToUnfollowDTO);

        author.Following.Remove(authorToUnfollow);
        authorToUnfollow.Followers.Remove(author);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// This method returns a list of Id's of Authors that an Author is following.
    /// It takes an AuthorDTO as a parameter and returns a list of Id's of
    /// the Authors that the AuthorDTO is following.
    /// </summary>
    /// <param name="authorDTO">The AuthorDTO for which to find the followed Authors</param>
    /// <returns>
    /// An IEnumerable of Author Id's
    /// </returns>
    public async Task<IEnumerable<string>> GetFollowing(AuthorDTO authorDTO)
    {
        var author = await FindAuthorByAuthorDTO(authorDTO);
        var following = new List<string>();

        foreach (var authorToFollow in author.Following)
        {
            following.Add(authorToFollow.Id);
        }

        return following;
    }

    /// <summary>
    /// This method returns a list of names of Authors that an Author is following.
    /// It takes an AuthorDTO as a parameter and returns a list of names of
    /// the Authors that the AuthorDTO is following.
    /// </summary>
    /// <param name="authorDTO">The AuthorDTO for which to find the followed Authors</param>
    /// <returns>
    /// An IEnumerable of Author names
    /// </returns>
    public async Task<IEnumerable<string>> GetFollowingNames(AuthorDTO authorDTO)
    {
        var author = await FindAuthorByAuthorDTO(authorDTO);
        var following = new List<string>();

        foreach (var authorToFollow in author.Following)
        {
            following.Add(authorToFollow.UserName);
        }

        return following;
    }

    /// <summary>
    /// This method takes an Author Id as parameter and deletes the Author with the specified Id.
    /// It starts by clearing the Author's Following list and then the Author's Followers list.
    /// The Author is then removed from the database. 
    /// The Cheeps posted by this Author are not deleted, they are instead anonymised. 
    /// </summary>
    /// <param name="authorId">The Id of the Author to be deleted</param>
    /// <returns>
    /// A boolean value indicating whether the Author was deleted or not.
    /// </returns>
    public async Task<bool> DeleteAuthor(string authorId)
    {
        var author = await context.Authors
            .Include(a => a.Following)
            .Include(a => a.Followers)
            .Include(a => a.Cheeps)
            .FirstOrDefaultAsync(a => a.Id == authorId);

        if (author == null)
        {
            return false;
        }

        author.Following.Clear();
        author.Followers.Clear();

        context.Authors.Remove(author);
        await context.SaveChangesAsync();
        return true;
    }

}