using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium.Providers
{
    public class TasksProvider : ITasksProvider
    {
        public Task DelegateTask<T>(Func<T, Task> task, IEnumerable<T> list)
        {
            List<Task> tasks = new List<Task>();

            foreach (var obj in list)
            {
                tasks.Add(task(obj));
            }

            return Task.WhenAll(tasks.ToArray());
        }
    }

    public interface ITasksProvider
    {
        public Task DelegateTask<T>(Func<T, Task> task, IEnumerable<T> list);
    }
}
