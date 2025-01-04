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
        public async Task AddUserAsync(User user) { 
           
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
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
        public async Task UpdateUserAsync(User user) { 
            var oldUser = await _context.Users.FindAsync(user.UserId);
            if (oldUser != null)
            {
                oldUser.Name = user.Name;
                oldUser.Email = user.Email;
                //Should update password too? then:
                if (!string.IsNullOrEmpty(user.Password)) { 
                    oldUser.Password = user.Password;
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

            }
            
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
