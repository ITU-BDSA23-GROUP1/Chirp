using Microsoft.EntityFrameworkCore;

namespace Chirp.EF;

public interface IRepository<T, in Tstring>
{
    public Task<IEnumerable<T>> Get(int offset);
    public Task<IEnumerable<T>> GetByFilter(string attribute, int offset);
}

public class CheepRepository : IRepository<CheepDTO, string>
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


}