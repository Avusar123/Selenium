using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
