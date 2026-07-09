namespace exemploNHibernate.Repositorio
{
    public interface IRepositorio<T>
    {
        Task Add(T item);
        Task Remove(long id);
        Task Update(T item);
        Task<T> FindByID(long id);
        IEnumerable<T> FindAll();
    }
}
