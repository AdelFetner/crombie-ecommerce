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
    internal class RoleService
    {
        private readonly ShopContext _shopContext;


        public RoleService(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }


        public async Task<Role> GetDefaultRole()
        {
            return await _shopContext.Roles.FirstOrDefaultAsync(r => r.Name == "Cliente")
                ?? throw new Exception("Rol por defecto no encontrado");
        }

        public async Task AssignRoleToUser(string username, int roleId)
        {
            var user = await _shopContext.Users.FirstOrDefaultAsync(u => u.Name == username);
            if (user != null)
            {
                user.RoleId = roleId;
                await _shopContext.SaveChangesAsync();
            }
        }
    }
}
