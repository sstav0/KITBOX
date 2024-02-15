namespace Kitbox_project;

public interface DBFunctions<T>
{
    Task<List<T>> GetList();
    Task<List<T>> GetById();
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
}
