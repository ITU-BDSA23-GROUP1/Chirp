namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository<CheepDTO, string>
{

    private readonly ChirpDBContext context;

    public CheepRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public Cheep Add(Cheep cheep)
    {
        return context.Cheeps.Add(cheep).Entity;
    }

    public async Task<IEnumerable<CheepDTO>> Get(int offset)
    {
        var cheeps = await context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Skip(offset)
            .Take(32)
            .Select(c => new CheepDTO
            {
                text = c.Text,
                timeStamp = c.TimeStamp,
                author = new AuthorDTO
                {
                    authorID = c.Author.AuthorId,
                    name = c.Author.Name,
                },
            })
            .ToListAsync();


        return cheeps;
    }


    public async Task<IEnumerable<CheepDTO>> GetByFilter(string authorName, int offset)
    {
        var cheeps = await context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDTO
            {
                text = c.Text,
                timeStamp = c.TimeStamp,
                author = new AuthorDTO
                {
                    authorID = c.Author.AuthorId,
                    name = c.Author.Name,
                },
            })
        .Where(c => c.author.name == authorName)
        .Skip(offset)
        .Take(32)
        .ToListAsync();

        return cheeps;
    }


    public async Task<AuthorDTO> FindAuthorByName(string name) 
    {
        var author = await context.Authors
            .Where(a => a.Name == name)
            .Select(a => new AuthorDTO
            {
                authorID = a.AuthorId,
                name = a.Name,
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
                authorID = a.AuthorId,
                name = a.Name,
            })
            .FirstOrDefaultAsync(); //Maybe delete this line

        return author;
    }

    public AuthorDTO CreateAuthor(int authorID, string name)
    {
        var author = new AuthorDTO
        {
            authorID = authorID,
            name = name,     
            };

        return author;
    }

    public CheepDTO CreateCheep(string text, DateTime timeStamp, AuthorDTO author)
    {
        AuthorDTO checkAuthor = FindAuthorByName(author.name).Result;
        if (FindAuthorByName(author.name) == null)
        {
            checkAuthor = CreateAuthor(author.authorID, author.name); 
        }

        var cheep = new CheepDTO
        {
            text = text,
            timeStamp = timeStamp,
            author = checkAuthor
        };

        return cheep;
    }


}