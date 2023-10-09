using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Chirp.EF;

public interface IRepository<T, in TFilter>
{
    public Task<IEnumerable<T>> Get(int offset);
    public Task<IEnumerable<T>> GetByFilter(TFilter attribute, int offset);
}

public class CheepRepository : IRepository<Cheep, Author>
{

    private readonly CheepContext context;

    public CheepRepository()
    {
        context = new CheepContext();
        //?? throw new ArgumentNullException(nameof(context));
    }

    public Cheep Add(Cheep cheep)
    {
        return context.cheeps.Add(cheep).Entity;
    }

    public async Task<IEnumerable<Cheep>> Get(int offset)
    {
        var cheeps = await context.cheeps
            .Take(32)
            .Skip(offset)
            .Select(c => new Cheep
            {
                cheepID = c.cheepID,
                text = c.text,
                timeStamp = c.timeStamp,
                author = new Author
                {
                    authorID = c.author.authorID,
                    name = c.author.name,
                    email = c.author.email
                },
            })
            .ToListAsync();


        return cheeps;
    }


    public async Task<IEnumerable<Cheep>> GetByFilter(Author author, int offset)
    {
        var cheeps = await context.cheeps
            .Take(32)
            .Skip(offset)
            .Select(c => new Cheep
            {
                cheepID = c.cheepID,
                text = c.text,
                timeStamp = c.timeStamp,
                author = new Author
                {
                    authorID = c.author.authorID,
                    name = c.author.name,
                    email = c.author.email
                },
            })
        .Where(c => c.author.authorID == author.authorID)
        .ToListAsync();

        return cheeps;
    }


}