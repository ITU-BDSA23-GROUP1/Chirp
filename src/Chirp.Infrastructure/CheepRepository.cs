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
        if (offset < 0) { offset = 0; }
        var cheeps = await context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Skip(offset)
            .Take(32)
            .Select(c => new CheepDTO
            {
                Id = c.CheepId.ToString(),
                Text = c.Text,
                TimeStamp = c.TimeStamp,
                Author = new AuthorDTO
                {
                    Id = c.Author.Id,
                    UserName = c.Author.UserName,
                    Email = c.Author.Email
                },
            })
            .ToListAsync();


        return cheeps;
    }


    public async Task<IEnumerable<CheepDTO>> GetByFilter(string authorName, int offset)
    {
        if (offset < 0) { offset = 0; }
        var cheeps = await context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDTO
            {
                Id = c.CheepId.ToString(),
                Text = c.Text,
                TimeStamp = c.TimeStamp,
                Author = new AuthorDTO
                {
                    Id = c.Author.Id,
                    UserName = c.Author.UserName,
                    Email = c.Author.Email
                },
            })
        .Where(c => c.Author.UserName == authorName)
        .Skip(offset)
        .Take(32)
        .ToListAsync();

        return cheeps;
    }

    public async Task<IEnumerable<CheepDTO>> GetByFollowers(List<string> authorNames, int offset)
    {
        if (offset < 0) { offset = 0; }
        var cheeps = await context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDTO
            {
                Id = c.CheepId.ToString(),
                Text = c.Text,
                TimeStamp = c.TimeStamp,
                Author = new AuthorDTO
                {
                    Id = c.Author.Id,
                    UserName = c.Author.UserName,
                    Email = c.Author.Email
                },
            })
        .Where(c => authorNames.Contains(c.Author.Id))
        .Skip(offset)
        .Take(32)
        .ToListAsync();

        return cheeps;
    }

    public async Task CreateCheep(CheepDTO cheepDTO)
    {

        AuthorRepository authorRepository = new AuthorRepository(context);

        Author cheepAuthor = await authorRepository.FindAuthorByAuthorDTO(cheepDTO.Author);

        Cheep cheep = new Cheep
        {
            Text = cheepDTO.Text,
            TimeStamp = cheepDTO.TimeStamp,
            Author = cheepAuthor,
            CheepId = Guid.NewGuid()
        };

        context.Cheeps.Add(cheep);
        await context.SaveChangesAsync();
    }


    public async Task<bool> DeleteCheep(string cheepID)
    {
        var cheep = await context.Cheeps
            .FirstOrDefaultAsync(c => c.CheepId.ToString() == cheepID);

        if (cheep == null)
        {
            return false;
        }

        context.Cheeps.Remove(cheep);
        await context.SaveChangesAsync();
        return true;
    }

}