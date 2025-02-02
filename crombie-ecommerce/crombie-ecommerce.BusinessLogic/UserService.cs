using crombie_ecommerce.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http;
using Interfaces;
using crombie_ecommerce.Models.Dto;

namespace crombie_ecommerce.BusinessLogic
{
    public class UserService
    {
        private readonly ShopContext _context;
        private readonly s3Service _s3Service;
        private readonly string _bucketFolder = "users";

        public UserService (ShopContext context, s3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }


         //Create a new user:
        public async Task<User> CreateUser(UserDto userDto, IFormFile fileImage) {

            var userId = Guid.NewGuid();

            var user = new User
            {
                UserId = userId,
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                Address = userDto.Address,
                Image = $"{_bucketFolder}/{userId}/{fileImage.FileName}"
            };

            using var stream = fileImage.OpenReadStream();

            var upload = await _s3Service.UploadFileAsync(stream, fileImage.FileName, fileImage.ContentType, $"{_bucketFolder}/{user.UserId}");

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
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

        // Method to delete and archive user
        public async Task<bool> ArchiveMethod(Guid userId, string processedBy = "Unregistered")
        {
            var user = await _context.Users
                .Include(u => u.Wishlists)
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return false;

            var historyUser = new HistoryUser
            {
                OriginalId = user.UserId,
                UserId = user.UserId,
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = processedBy,
                EntityJson = user.SerializeToJson()
            };

            _context.HistoryUsers.Add(historyUser);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
