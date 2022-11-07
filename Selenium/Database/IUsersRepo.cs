using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Database
{
    public interface IUsersRepo : IAsyncCRUD<UserData>, IEntityFinder<UserData, int>
    {

        public Task<List<UserData>> GetAll();

        public Task<List<UserData>> GetAll(Expression<Func<UserData, bool>> predicate);

        public Task RemoveRange(Expression<Func<UserData, bool>> predicate);
    }
}
