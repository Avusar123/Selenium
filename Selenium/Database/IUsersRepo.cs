using System.Linq.Expressions;

namespace Selenium.Database
{
    public interface IUsersRepo : IAsyncCRUD<UserData>, IEntityFinder<UserData, int>
    {

        public Task<List<UserData>> GetAll();

        public Task<List<UserData>> GetAll(Expression<Func<UserData, bool>> predicate);

        public Task RemoveRange(Expression<Func<UserData, bool>> predicate);
    }
}
