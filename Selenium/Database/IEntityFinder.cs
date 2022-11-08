using System.Linq.Expressions;

namespace Selenium.Database
{
    public interface IEntityFinder<T, IdType>
    {
        public Task<T> Get(Expression<Func<T, bool>> predicate);

        public Task<T> GetById(IdType id);
    }
}
