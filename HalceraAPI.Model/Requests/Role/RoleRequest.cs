using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Role
{
    public class RoleRequest
    {
        /// <summary>
        /// Role Id
        /// </summary>
        [Required]
        public int Id { get; set; }
    }
}
