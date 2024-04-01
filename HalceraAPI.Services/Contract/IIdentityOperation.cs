﻿using HalceraAPI.Models;
using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.ApplicationUser;
using HalceraAPI.Services.Dtos.RefreshToken;
using HalceraAPI.Services.Dtos.Role;

namespace HalceraAPI.Services.Contract
{
    public interface IIdentityOperation
    {
        Task<UserAuthResponse> Register(RegisterRequest registerRequest);
        Task<UserAuthResponse> Login(LoginRequest loginRequest);
        Task<UserAuthResponse> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<ApplicationUser> GetLoggedInUserAsync();
        Task<IEnumerable<RoleResponse>> GetApplicationRoles();
        Task<ApplicationUser?> GetUserWithEmail(string? email);
    }
}
