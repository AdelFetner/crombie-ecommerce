using crombie_ecommerce.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using crombie_ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Verificar si el usuario que realiza la solicitud es un administrador
            var adminUser = await _shopContext.Users
                .Include(u => u.Role) // Asegúrate de incluir el rol del administrador
                .FirstOrDefaultAsync(u => u.Email == adminEmail);

            if (adminUser == null || adminUser.Role?.Name != "Admin")
            {
                throw new UnauthorizedAccessException("Solo los administradores pueden asignar roles.");
            }

            // Buscar al usuario al que se le asignará el rol por su email
            var targetUser = await _shopContext.Users.FirstOrDefaultAsync(u => u.Email == targetUserEmail);
            if (targetUser == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            // Verificar si el rol que se está asignando existe
            var role = await _shopContext.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
            if (role == null)
            {
                throw new Exception("Rol no encontrado.");
            }

            // Asignar el rol al usuario
            targetUser.RoleId = roleId;
            await _shopContext.SaveChangesAsync();
        }

    }
}

