using crombie_ecommerce.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("assign-role")]
        [Authorize] 
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            try
            {
               
                var adminEmail = User.Identity?.Name;

                if (string.IsNullOrEmpty(adminEmail))
                {
                    return Unauthorized("User Not Authenticated.");
                }

                
                await _roleService.AssignRoleToUser(adminEmail, request.TargetUserEmail, request.RoleId);

                return Ok("Role assigned successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class AssignRoleRequest
    {
        public string TargetUserEmail { get; set; } 
        public int RoleId { get; set; }
    }
}


