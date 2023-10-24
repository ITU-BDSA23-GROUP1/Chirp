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
                Text = c.Text,
                TimeStamp = c.TimeStamp,
                Author = new AuthorDTO
                {
                    AuthorId = c.Author.AuthorId,
                    Name = c.Author.Name,
                    Email = c.Author.Email
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
                Text = c.Text,
                TimeStamp = c.TimeStamp,
                Author = new AuthorDTO
                {
                    AuthorId = c.Author.AuthorId,
                    Name = c.Author.Name,
                    Email = c.Author.Email
                },
            })
        .Where(c => c.Author.Name == authorName)
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

    private Author FindAuthorByAuthorDTO(AuthorDTO authorDTO)
    {
        var author = context.Authors
            .Where(a => a.AuthorId == authorDTO.AuthorId)
            .FirstOrDefault();

        return author;
    }

    public AuthorDTO CreateAuthor(string name, string email)
    {
        var author = new Author
        {
            AuthorId = Guid.NewGuid(),
            Name = name,
            Email = email,
            Cheeps = new List<Cheep>()
        };

        context.Authors.Add(author);
        context.SaveChanges();

        return new AuthorDTO { AuthorId = author.AuthorId, Name = author.Name, Email = author.Email };
    }

    public void CreateCheep(string text, DateTime timeStamp, AuthorDTO author)
    {
        AuthorDTO checkAuthor = FindAuthorByName(author.Name).Result;
        if (checkAuthor == null)
        {
            checkAuthor = CreateAuthor(author.Name, author.Email);
        }

        var cheepAuthor = FindAuthorByAuthorDTO(checkAuthor);

        var cheep = new Cheep
        {
            Text = text,
            TimeStamp = timeStamp,
            Author = cheepAuthor,
            CheepId = Guid.NewGuid(),
            AuthorId = cheepAuthor.AuthorId
        };

        context.Cheeps.Add(cheep);
        context.SaveChanges();
    }


}