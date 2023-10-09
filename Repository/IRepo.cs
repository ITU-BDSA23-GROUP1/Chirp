namespace Repositories;

public interface IRepository<out T, in TFilter>
{
    public IEnumerable<T> Get(int offset);
    public IEnumerable<T> GetByFilter(TFilter attribute, int offset);
}