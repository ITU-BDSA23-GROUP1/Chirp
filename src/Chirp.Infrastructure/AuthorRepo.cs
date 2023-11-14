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




}