namespace HalceraAPI.Services.Dtos.ApplicationUser
{
    public class UserResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool Active { get; set; }
        public bool? AccountDeleted { get; set; }
    }
}
