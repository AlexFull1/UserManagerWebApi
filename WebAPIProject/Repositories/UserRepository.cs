using Microsoft.EntityFrameworkCore;
using WebAPIProject.Models;

namespace WebAPIProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        public UserRepository(ApplicationContext context) => _context = context;
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync() =>    
            await _context.Users.ToListAsync();
        

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user ?? throw new InvalidOperationException($"User with {id} is not found!");
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);

            existingUser.Name = user.Name;
            existingUser.Age = user.Age;
            existingUser.Balance = user.Balance;
            
            await _context.SaveChangesAsync();
        }
    }
}
