using crombie_ecommerce.Models;
using crombie_ecommerce.Contexts;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Services
{
    public class UserService
    {
        private readonly ShopContext _context;

        public UserService (ShopContext context)
        {
            _context = context;
        }


         //Create a new user:
        public async Task<User> PostUser(User user) { 
           
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
            
        }


        //Read user by id:
        public async Task<User> GetUserByIdAsync(Guid id) { 
            return await _context.Users.FindAsync(id);
        }

        //Read all users:
        public async Task<List<User>> GetAllUsersAsync() {
            return await _context.Users.ToListAsync();
        }


        //Update user:
        public async Task<User> UpdateUserAsync(Guid Id, User updatedUser) {
            if (string.IsNullOrEmpty(updatedUser.Name))
            {
                throw new ArgumentException("User name is required.");
            }

            // look up for the first user with the given id by reusing previous get by id method
            var existingUser = await GetUserByIdAsync(Id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            // update
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.Password = updatedUser.Password; 
            existingUser.IsVerified = updatedUser.IsVerified;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            return existingUser;

        }

        //Delete user:
        public async Task DeleteUserAsync(Guid id) {
            var user2 = await _context.Users.FindAsync(id);
            if (user2 != null)
            {
                _context.Users.Remove(user2);
                await _context.SaveChangesAsync();
                
            }
            
        }
    }
}
