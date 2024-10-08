﻿using HalceraAPI.Services.Dtos.RefreshToken;
using HalceraAPI.Services.Dtos.Role;

namespace HalceraAPI.Services.Dtos.ApplicationUser
{
    /// <summary>
    /// User response
    /// </summary>
    public class UserAuthResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool? Active { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string? Token { get; set; }
        public RefreshTokenResponse? RefreshToken { get; set; }
        public ICollection<RoleResponse>? Roles { get; set; }
    }
}
