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
        Console.WriteLine("Name: " + UserName);
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

    public Author FindAuthorByAuthorDTO(AuthorDTO authorDTO)
    {
        var author = context.Authors
            .Where(a => a.Id == authorDTO.Id)
            .FirstOrDefault();

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
        var author = await context.Authors.Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == authorDTO.Id);
       // var author = FindAuthorByAuthorDTO(authorDTO);
        Console.WriteLine("Author: " + author.UserName);
        //var authorToFollow = FindAuthorByAuthorDTO(authorToFollowDTO);
        var authorToFollow = await context.Authors.Include(u => u.Followers).FirstOrDefaultAsync(u => u.Id == authorToFollowDTO.Id);
        Console.WriteLine("AuthorToFollow: " + authorToFollow.UserName);


        
        author.Following.Add(authorToFollow);
        authorToFollow.Followers.Add(author);
        //context.Authors.Update(author);
        //context.SaveChanges();
        await context.SaveChangesAsync();
    }

    public async Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO)
    {
        var author = FindAuthorByAuthorDTO(authorDTO);
        var authorToUnfollow = FindAuthorByAuthorDTO(authorToUnfollowDTO);

        author.Following.Remove(authorToUnfollow);
        context.SaveChanges();
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetFollowing(AuthorDTO authorDTO) {
        var author = await context.Authors.Include(u => u.Following).FirstOrDefaultAsync(u => u.Id == authorDTO.Id);
        var following = new List<string>();

        foreach(var authorToFollow in author.Following) {
            /*var authorToFollowDTO = new AuthorDTO {
                Id = authorToFollow.Id,
                UserName = authorToFollow.UserName,
                Email = authorToFollow.Email
            };*/
            following.Add(authorToFollow.Id);
            //Console.WriteLine("AuthorToFollow: " + authorToFollow.UserName);
        }

        return following;
    }

}