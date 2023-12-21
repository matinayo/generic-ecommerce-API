using AutoMapper;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.BaseAddress;
using HalceraAPI.Services.Contract;
using System.Linq.Expressions;

namespace HalceraAPI.Services.Operations
{
    public class UserOperation : IUserOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityOperation _identityOperation;
        private readonly IMapper _mapper;

        public UserOperation(IUnitOfWork unitOfWork, IIdentityOperation identityOperation, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identityOperation = identityOperation;
            _mapper = mapper;
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

        public async Task<APIResponse<UserResponse>> UpdateUserDetailsAsync(string userId, UpdateUserRequest updateUserRequest)
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
                UserResponse userResponse = _mapper.Map<UserResponse>(user);

                return new APIResponse<UserResponse>(userResponse);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<UserResponse>> GetUserByIdAsync(string userId)
        {
            try
            {
                UserResponse userDetails = await _unitOfWork.ApplicationUser.GetFirstOrDefault<UserResponse>(
                    user => user.Id.ToLower().Equals(userId.ToLower()))
                    ?? throw new Exception("This user cannot be found");

                return new APIResponse<UserResponse>(userDetails);
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
            catch (Exception e)
            {
                throw;
            }
        }


        public async Task LockUnlockUserAsync(string userId, AccountAction accountAction)
        {
            try
            {
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
    }
}
