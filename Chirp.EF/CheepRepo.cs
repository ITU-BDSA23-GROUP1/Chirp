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
        context.Add(new Cheep
        {
            Text = "Hello World!",
            TimeStamp = DateTime.Now,
            Author = new Author { Name = "John Doe", Email = "mgon@itu.dk" }
        });
        context.SaveChanges();
        //?? throw new ArgumentNullException(nameof(context));
    }

    public Cheep Add(Cheep cheep)
    {
        return context.Cheeps.Add(cheep).Entity;
    }

    public async Task<IEnumerable<CheepDTO>> Get(int offset)
    {
        var cheeps = await context.Cheeps
            .Take(32)
            .Skip(offset)
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
            .OrderByDescending(c => c.timeStamp)
            .ToListAsync();


        return cheeps;
    }


    public async Task<IEnumerable<CheepDTO>> GetByFilter(string authorID, int offset)
    {
        var cheeps = await context.Cheeps
            .Take(32)
            .Skip(offset)
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
        .Where(c => c.author.authorID == Int32.Parse(authorID))
        .OrderByDescending(c => c.timeStamp)
        .ToListAsync();

        return cheeps;
    }


}