namespace Selenium.Database
{
    public interface IAsyncCRUD<T>
    {
        public Task<T> Create(T entity);

        public Task Delete(T entity);

        public Task Update(T entity);

        public Task Save();
    }
}
