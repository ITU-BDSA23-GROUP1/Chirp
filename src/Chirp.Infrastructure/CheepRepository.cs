namespace Chirp.Infrastructure;

/// <summary>
/// This class represents a repository for Cheeps.
/// It implements the ICheepRepository interface.
/// </summary>
public class CheepRepository : ICheepRepository
{

    private readonly ChirpDBContext context;

    public CheepRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// This method returns all Cheeps in the database within a specific range.
    /// The range is specified by the offset parameter which is calculated as (PageNo-1)*32.
    /// This makes it possible to implement pagination with 32 Cheeps per page.
    /// </summary>
    /// <param name="offset">The offset for pagination</param>
    /// <returns>
    /// An IEnumerable of CheepDTOs within the specified range.
    /// </returns>
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


    /// <summary>
    /// This method returns all Cheeps in the database written by a specific Author
    /// and within a specific range.
    /// The range is specified by the offset parameter which is calculated as (PageNo-1)*32.
    /// This makes it possible to implement pagination with 32 Cheeps per page.
    /// </summary>
    /// <param name="authorName">The name of the author</param>
    /// <param name="offset">The offset for pagination</param>
    /// <returns>
    /// An IEnumerable of CheepDTOs written by the author specified by the 
    /// <paramref name="authorName"/> parameter and within the specified range.
    /// </returns>
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

    /// <summary>
    /// This method returns all Cheeps in the database written by specific Authors.
    /// It takes a list of Author ids as a parameter and returns all Cheeps written by
    /// the Authors in the list. The Cheeps are returned within a specific range.
    /// The method is used for the private timeline of an authenticated user, where the user can see
    /// all Cheeps written by the Authors that the user is following.
    /// </summary>
    /// <param name="authorIds">The ids of the authors that the user is following</param>
    /// <param name="offset">The offset for pagination</param>
    /// <returns>
    /// An IEnumerable of CheepDTOs written by the Authors specified by the 
    /// <paramref name="authorIds"/> parameter and within the specified range. 
    /// </returns>
    public async Task<IEnumerable<CheepDTO>> GetByFollowers(List<string> authorIds, int offset)
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
        .Where(c => authorIds.Contains(c.Author.Id))
        .Skip(offset)
        .Take(32)
        .ToListAsync();

        return cheeps;
    }

    /// <summary>
    /// This method adds a new Cheep to the database.
    /// It takes a CheepDTO as a parameter and creates a new Cheep by finding the 
    /// Author corresponding to the CheepDTO's AuthorDTO, and then mapping the CheepDTO to a Cheep.
    /// The Cheep is then added to the database.
    /// </summary>
    /// <param name="cheepDTO">The CheepDTO to be added to the database</param>
    /// <returns></returns>
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

    /// <summary>
    /// This method deletes a Cheep from the database.
    /// It takes a CheepID as a parameter and finds the corresponding Cheep in the database.
    /// If the Cheep is found, it is deleted from the database.
    /// </summary>
    /// <param name="cheepID">The ID of the Cheep to be deleted</param>
    /// <returns>
    /// A boolean indicating whether the Cheep was deleted or not.
    /// </returns>
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