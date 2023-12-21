using AutoMapper;
using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.BaseAddress;
using HalceraAPI.Models.Requests.Role;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Security.Claims;

namespace HalceraAPI.Services.Operations
{
    public class UserOperation : IUserOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityOperation _identityOperation;
        private readonly IMapper _mapper;
        private readonly JWTOptions jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserOperation(
            IUnitOfWork unitOfWork,
            IIdentityOperation identityOperation, 
            IMapper mapper,
            IOptions<JWTOptions> options, 
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _identityOperation = identityOperation;
            _mapper = mapper;
            jwtOptions = options.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task DeleteAccountAsync(string userId)
        {
            try
            {
                ApplicationUser userDetails = await _unitOfWork.ApplicationUser.GetFirstOrDefault(
                    user => user.Id.ToLower().Equals(userId.ToLower()))
                    ?? throw new Exception("This user cannot be found");

                userDetails.DeleteUserAccount();
                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<UserDetailsResponse>> UpdateUserDetailsAsync(string userId, UpdateUserRequest updateUserRequest)
        {
            try
            {
                if (updateUserRequest.Email != null)
                {
                    var emailIdentity = await _identityOperation.GetUserWithEmail(updateUserRequest.Email);
                    if (emailIdentity != null)
                    {
                        throw new Exception("This email is already in use");
                    }
                }

                ApplicationUser user = await _unitOfWork.ApplicationUser.GetFirstOrDefault(
                    user => user.Id.ToLower().Equals(userId.ToLower()))
                    ?? throw new Exception("This user cannot be found");

                _mapper.Map(updateUserRequest, user);
                user.FormatUserEmail();

                await _unitOfWork.SaveAsync();
                UserDetailsResponse userResponse = _mapper.Map<UserDetailsResponse>(user);

                return new APIResponse<UserDetailsResponse>(userResponse);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<UserDetailsResponse>> GetUserByIdAsync(string userId)
        {
            try
            {
                UserDetailsResponse userDetails = await _unitOfWork.ApplicationUser.GetFirstOrDefault<UserDetailsResponse>(
                    user => user.Id.ToLower().Equals(userId.ToLower()))
                    ?? throw new Exception("This user cannot be found");

                return new APIResponse<UserDetailsResponse>(userDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<IEnumerable<UserResponse>>> GetUsersAsync(int? roleId, bool? active, bool? deleted, int? page)
        {
            try
            {
                Expression<Func<ApplicationUser, bool>>? filterExpression = null;
                if (deleted.HasValue)
                {
                    filterExpression = user => user.AccountDeleted == deleted.Value;
                }
                else if (roleId != null && active.HasValue)
                {
                    filterExpression = user => user.Roles != null
                                        && user.Roles.Any(role => role.Id == roleId)
                                        && user.Active == active.Value;
                }
                else if (roleId != null)
                {
                    filterExpression = user => user.Roles != null
                                        && user.Roles.Any(role => role.Id == roleId);
                }
                else if (active.HasValue)
                {
                    filterExpression = user => user.Active == active.Value;
                }

                int totalItems = await _unitOfWork.ApplicationUser.CountAsync(filterExpression);
                IEnumerable<UserResponse> listOfUsersResponse =
                    await _unitOfWork.ApplicationUser.GetAll<UserResponse>(
                        filter: filterExpression,
                        skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                        take: Pagination.DefaultItemsPerPage);

                var meta = new Meta(totalItems, Pagination.DefaultItemsPerPage, page ?? 1);

                return new APIResponse<IEnumerable<UserResponse>>(listOfUsersResponse, meta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<AddressResponse>> UpdateAddressAsync(string userId, AddressRequest updateAddressRequest)
        {
            try
            {
                ApplicationUser user = await _unitOfWork.ApplicationUser.GetFirstOrDefault(
                    user => user.Id.ToLower().Equals(userId.ToLower()), 
                    includeProperties: nameof(ApplicationUser.Address)) 
                    ?? throw new Exception("This user cannot be found");

                _mapper.Map(updateAddressRequest, user.Address ??= new());
                await _unitOfWork.SaveAsync();

                var addressResponse = _mapper.Map<AddressResponse>(user.Address);

                return new APIResponse<AddressResponse>(addressResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task LockUnlockUserAsync(string userId, AccountAction accountAction)
        {
            try
            {
                IdenticalUserCannotModify(userId);
                ApplicationUser applicationUser = await _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(user => user.Id.ToLower().Equals(userId.ToLower()))
                    ?? throw new Exception("This user cannot be found");

                if (applicationUser.LockoutEnd != null && applicationUser.LockoutEnd > DateTime.UtcNow)
                {
                    applicationUser.LockoutEnd = DateTime.UtcNow;
                    applicationUser.Active = true;
                }
                else
                {
                    applicationUser.LockoutEnd = DateTime.UtcNow.AddYears(1000);
                    applicationUser.Active = false;
                }

                await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<UserAuthResponse>> DeleteRoleFromUserAsync(string userId, int roleId)
        {
            try
            {
                IdenticalUserCannotModify(userId);
                ApplicationUser applicationUser = await _unitOfWork.ApplicationUser.GetFirstOrDefault(
                    user => user.Id.ToLower().Equals(userId.ToLower()),
                    includeProperties: nameof(ApplicationUser.Roles))
                ?? throw new Exception("This user cannot be found");

                if(applicationUser.Roles == null || !applicationUser.Roles.Any())
                {
                    throw new Exception("No roles available for this user");
                }

                Roles role = applicationUser.Roles.FirstOrDefault(role => role.Id == roleId) 
                    ?? throw new Exception("This role is invalid");
                
                applicationUser.Roles.Remove(role);
                if (!applicationUser.Roles.Any())
                {
                    Roles? customerRoleFromDb = await _unitOfWork.Roles.GetFirstOrDefault(role =>
                    role.Title.ToLower().Equals(RoleDefinition.Customer.ToLower()));

                    if (customerRoleFromDb != null)
                    {
                        applicationUser.Roles.Add(customerRoleFromDb);
                    }
                }
                await _unitOfWork.SaveAsync();

                UserAuthResponse userResponse = _mapper.Map<UserAuthResponse>(applicationUser);
                userResponse.Token = JWTManager.CreateToken(applicationUser, jwtOptions.Token);

                return new APIResponse<UserAuthResponse>(userResponse);
            }catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<UserAuthResponse>> UpdateUserRoleUserAsync(string userId, int roleId)
        {
            try
            {
                IdenticalUserCannotModify(userId);
                ApplicationUser applicationUser = await _unitOfWork.ApplicationUser.GetFirstOrDefault(
                    user => user.Id.ToLower().Equals(userId.ToLower()),
                    includeProperties: nameof(ApplicationUser.Roles))
                    ?? throw new Exception("This user cannot be found");

                if (applicationUser.Roles == null || !applicationUser.Roles.Any())
                {
                    throw new Exception("No roles available for this user");
                }

                Roles role = await _unitOfWork.Roles.GetFirstOrDefault(role => role.Id == roleId)
                    ?? throw new Exception("This role is invalid");

                applicationUser.Roles.Add(role);
                await _unitOfWork.SaveAsync();
                UserAuthResponse userResponse = _mapper.Map<UserAuthResponse>(applicationUser);
                userResponse.Token = JWTManager.CreateToken(applicationUser, jwtOptions.Token);

                return new APIResponse<UserAuthResponse>(userResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void IdenticalUserCannotModify(string userId)
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var claim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    if (claim.Value.ToLower().Equals(userId.ToLower()))
                    {
                        throw new Exception("You cannot modify your own role");
                    }
                }
            }
        }
    }
}
