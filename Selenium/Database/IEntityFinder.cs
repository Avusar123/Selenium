using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Database
{
    public interface IEntityFinder<T, IdType>
    {
        public Task<T> Get(Expression<Func<T, bool>> predicate);

        public Task<T> GetById(IdType id);
    }
}
