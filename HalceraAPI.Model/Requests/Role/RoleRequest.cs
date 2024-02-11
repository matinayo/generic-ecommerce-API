using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Role
{
    public class RoleRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
