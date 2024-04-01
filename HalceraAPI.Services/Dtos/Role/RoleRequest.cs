using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Role
{
    public class RoleRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
