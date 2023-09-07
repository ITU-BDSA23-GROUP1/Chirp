namespace SimpleDB;

public interface IDatabaseRepository<T>{

    public string fileName { get; set; }
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
}