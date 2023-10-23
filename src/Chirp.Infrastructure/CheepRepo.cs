namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository<CheepDTO, string>
{

    private readonly ChirpDBContext context;

    public CheepRepository(ChirpDBContext context)
    {
        this.context = context;
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

    private Author FindAuthorByAuthorDTO(AuthorDTO authorDTO)
    {
        var author = context.Authors
            .Where(a => a.AuthorId == authorDTO.authorID)
            .FirstOrDefault();

        return author;
    }

    public Author CreateAuthor(int authorID, string name, string email)
    {
        var author = new Author
        {
            AuthorId = authorID,
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>()
        };
        return context.Authors.Add(author).Entity;
    }

    public Cheep CreateCheep(int CheepId, string text, DateTime timeStamp, int AuthorId, Author author)
    {
        Author checkAuthor = FindAuthorByAuthorDTO(FindAuthorByName(author.Name).Result);
        if (FindAuthorByName(author.Name) == null)
        {
            checkAuthor = CreateAuthor(author.AuthorId, author.Name, author.Email); 
        }

        var cheep = new Cheep
        {
            Text = text,
            TimeStamp = timeStamp,
            Author = checkAuthor,
            CheepId = CheepId,
            AuthorId = AuthorId
        };

        return cheep;
    }


}