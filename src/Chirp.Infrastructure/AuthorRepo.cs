using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository<AuthorDTO, string>
{

    private readonly ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

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


    public async Task<AuthorDTO> FindAuthorByEmail(string email)
    {
        var author = await context.Authors
            .Where(a => a.Email == email)
            .Select(a => new AuthorDTO
            {
                Id = a.Id,
                UserName = a.UserName,
                Email = a.Email
            })
            .FirstOrDefaultAsync(); //Maybe delete this line

        return author;
    }

    public async Task<Author> FindAuthorByAuthorDTO(AuthorDTO authorDTO)
    {
        var author = await context.Authors
            .Include(a => a.Following)
            .FirstOrDefaultAsync(a => a.Id == authorDTO.Id);

        return author;
    }

    public void CreateAuthor(AuthorDTO authorDTO)
    {
        var author = new Author
        {
            Id = authorDTO.Id,
            UserName = authorDTO.UserName,
            Email = authorDTO.Email,
            Cheeps = new List<Cheep>()
        };

        context.Authors.Add(author);
        context.SaveChanges();
    }

    public async Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO)
    {
        var author = await FindAuthorByAuthorDTO(authorDTO);
        var authorToFollow = await FindAuthorByAuthorDTO(authorToFollowDTO);

        author.Following.Add(authorToFollow);
        authorToFollow.Followers.Add(author);
        await context.SaveChangesAsync();
    }

    public async Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO)
    {
        var author = await FindAuthorByAuthorDTO(authorDTO);
        var authorToUnfollow = await FindAuthorByAuthorDTO(authorToUnfollowDTO);

        author.Following.Remove(authorToUnfollow);
        authorToUnfollow.Followers.Remove(author);
        await context.SaveChangesAsync();
    }

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

    public async Task<bool> DeleteAuthor(String authorId)
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
        //author.Cheeps.Clear();

        context.Authors.Remove(author);
        await context.SaveChangesAsync();
        return true;
    }

}