using crombie_ecommerce.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using crombie_ecommerce.Models.Entities;

namespace crombie_ecommerce.BusinessLogic
{
    public class RoleService
    {
        private readonly ShopContext _shopContext;

        public RoleService(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<Role> GetDefaultRole()
        {
            return await _shopContext.Roles.FirstOrDefaultAsync(r => r.Name == "User")
                ?? throw new Exception("Default Role not found");
        }

        public async Task AssignRoleToUser(string adminEmail, string targetUserEmail, int roleId)
        {
            // Checks for user role
            var adminUser = await _shopContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == adminEmail);

            if (adminUser == null || adminUser.Role?.Name != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can assign roles");
            }

            // Look by user email the user to assign the role
            var targetUser = await _shopContext.Users.FirstOrDefaultAsync(u => u.Email == targetUserEmail);
            if (targetUser == null)
            {
                throw new Exception("User not found");
            }

            // Checks if role exists
            var role = await _shopContext.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
            if (role == null)
            {
                throw new Exception("Role not found");
            }

            // Assigns role to user
            targetUser.RoleId = roleId;
            await _shopContext.SaveChangesAsync();
        }

    }
}

