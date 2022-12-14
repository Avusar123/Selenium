using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Selenium.Database
{
    public class UsersRepo : IUsersRepo
    {
        private AccountContext context;

        public UsersRepo(AccountContext context)
        {
            this.context = context;
        }

        public async Task<UserData> Create(UserData entity)
        {
            await context.Users.AddAsync(entity);

            await Save();

            return entity;
        }

        public async Task Delete(UserData entity)
        {
            context.Users.Remove(entity);

            await Save();
        }

        public async Task<UserData> Get(Expression<Func<UserData, bool>> predicate)
        {
            return await context.Users.FirstOrDefaultAsync(predicate);
        }

        public async Task<List<UserData>> GetAll()
        {
            return await context.Users.OrderByDescending(user => user.Balance).ToListAsync();
        }

        public async Task<List<UserData>> GetAll(Expression<Func<UserData, bool>> predicate)
        {
            return await context.Users.OrderByDescending(user => user.Balance).Where(predicate).ToListAsync();
        }

        public async Task<UserData> GetById(int id)
        {
            return await Get(user => user.Id == id);
        }

        public async Task RemoveRange(Expression<Func<UserData, bool>> predicate)
        {
            var users = context.Users.Where(predicate);

            context.Users.RemoveRange(users);

            await Save();

        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }

        public async Task Update(UserData entity)
        {
            context.Users.Update(entity);
            await Save();
        }
    }
}
