using Chirp.EF;

public class CheepRepository : IRepository<Cheep, Author>
{

    private readonly CheepContext context;

     public CheepRepository(OrderingContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));
        }

     public Cheep Add(Cheep cheep)
        {
            return context.cheeps.Add(cheep).Entity;
        }

    public async Task<Cheep> GetCheepsAsync(int offset)
        {
            var cheeps = await
                .From c in context.cheeps
                .limit(32).offset(offset)
                .Select(new Cheep(author: new Author(authorID: c.author.authorID, name: c.author.name, email: c.author.email), text: c.text, timeStamp: c.timeStamp))
                .ToListAsync()

            return cheeps;
        }


     public async Task<Cheep> GetCheepsByAuthorAsync(string authorID, int offset)
        {
            var cheeps = await
                .From c in context.cheeps
                .limit(32).offset(offset)
                .Where (c => c.authorID == authorID)
                .Select(new Cheep(author: new Author(authorID: c.author.authorID, name: c.author.name, email: c.author.email), text: c.text, timeStamp: c.timeStamp))
                .ToListAsync()

            return cheeps;
        }


}