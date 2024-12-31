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
        public User AddUser(User user) { 
           
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }


        //Read user by id:
        public User GetUserById(int id) { 
            return _context.Users.Find(id);
        }

        //Read all users:
        public List<User> GetAllUsers() {
            return _context.Users.ToList();
        }


        //Update user:
        public User UpdateUser(User user) { 
            var oldUser = _context.Users.Find(user.UserId);
            if (oldUser != null)
            {
                oldUser.Name = user.Name;
                oldUser.Email = user.Email;
                //Should update password too? then:
                if (!string.IsNullOrEmpty(user.Password)) { 
                    oldUser.Password = user.Password;
                }

                _context.Users.Update(user);
                _context.SaveChanges();

            }
            return user;
        }

        //Delete user:
        public void DeleteUser(int id) {
            var user2 = _context.Users.Find(id);
            if (user2 != null)
            {
                _context.Users.Remove(user2);
                _context.SaveChanges();
                
            }
            
        }
    }
}
