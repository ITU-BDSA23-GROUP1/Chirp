namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository<AuthorDTO, string>
{

    private readonly ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async Task<AuthorDTO> FindAuthorByName(string name)
    {
        Console.WriteLine("Name: " + name);
        var author = await context.Authors
            .Where(a => a.Name == name)
            .Select(a => new AuthorDTO
            {
                AuthorId = a.AuthorId,
                Name = a.Name,
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
                AuthorId = a.AuthorId,
                Name = a.Name,
                Email = a.Email
            })
            .FirstOrDefaultAsync(); //Maybe delete this line

        return author;
    }

    public Author FindAuthorByAuthorDTO(AuthorDTO authorDTO)
    {
        var author = context.Authors
            .Where(a => a.AuthorId == authorDTO.AuthorId)
            .FirstOrDefault();

        return author;
    }

    public void CreateAuthor(AuthorDTO authorDTO)
    {
        var author = new Author
        {
            AuthorId = authorDTO.AuthorId,
            Name = authorDTO.Name,
            Email = authorDTO.Email,
            Cheeps = new List<Cheep>()
        };

        context.Authors.Add(author);
        context.SaveChanges();

    }




}