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
        if (offset < 0) { offset = 0;}
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
        if (offset < 0) { offset = 0;}
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

    public void CreateCheep(CheepDTO cheepDTO)
    {

        AuthorRepository authorRepository = new AuthorRepository(context);

        AuthorDTO checkAuthor = cheepDTO.Author;
        if (authorRepository.FindAuthorByName(checkAuthor.Name).Result == null)
        {
            authorRepository.CreateAuthor(checkAuthor);
        }

        Author cheepAuthor = authorRepository.FindAuthorByAuthorDTO(checkAuthor);

        Cheep cheep = new Cheep
        {
            Text = cheepDTO.Text,
            TimeStamp = cheepDTO.TimeStamp,
            Author = cheepAuthor,
            CheepId = Guid.NewGuid(),
            AuthorId = cheepAuthor.AuthorId
        };

        context.Cheeps.Add(cheep);
        context.SaveChanges();
    }


}