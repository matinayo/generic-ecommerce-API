using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Services.Contract;
using System.Text.RegularExpressions;

namespace HalceraAPI.Services.Operations
{
    public class ApplicationUserOperation : IApplicationUserOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationUserOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResponse> Register(RegisterRequest registerRequest)
        {
            try
            {
                // validate if user email is already existing
                ApplicationUser? applicationUser = await GetUserWithEmail(registerRequest.Email);
                if (applicationUser != null)
                {
                    throw new Exception("The email address entered is already being used. Please select another.");
                }

                string passwordHash = ValidateAndCreateUserPassword(registerRequest.Password, registerRequest.ConfirmPassword);
                applicationUser = new()
                {
                    PasswordHash = passwordHash
                };
                _mapper.Map(registerRequest, applicationUser);

                await _unitOfWork.ApplicationUser.Add(applicationUser);
                await _unitOfWork.SaveAsync();

                UserResponse userResponse = _mapper.Map<UserResponse>(applicationUser);
                return userResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                ApplicationUser? applicationUserFromDb = await GetUserWithEmail(loginRequest.Email);
                if (applicationUserFromDb == null)
                {
                    throw new Exception("Incorrect email or password.");
                }
                VerifyPassword(loginRequest.Password, applicationUserFromDb.PasswordHash);
                applicationUserFromDb.LastLoginDate = DateTime.UtcNow;
                await _unitOfWork.SaveAsync();

                UserResponse userResponse = _mapper.Map<UserResponse>(applicationUserFromDb);
                return userResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Validate if user email is already in use
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>ApplicaitonUser associated with email</returns>
        private async Task<ApplicationUser?> GetUserWithEmail(string? email)
        {

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Email is required.");
            }

            ApplicationUser? userFromDb = await _unitOfWork.ApplicationUser
                .GetFirstOrDefault(user => user.Email.Trim().ToLower().Equals(email.Trim().ToLower()));
            return userFromDb;
        }

        public bool ValidateUsingRegex(string emailAddress)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);
            return regex.IsMatch(emailAddress);
        }

        /// <summary>
        /// Validate user password request, and create Password Hash
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="confirmPassword">Confirm password</param>
        /// <returns>Password Hash</returns>
        private static string ValidateAndCreateUserPassword(string? password, string? confirmPassword)
        {
            ValidatePassword(password, confirmPassword);
            string passwordHash = CreatePasswordHash(password!);
            return passwordHash;
        }

        /// <summary>
        /// Validate password and password format
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="confirmPassword">Confirm Password</param>
        private static void ValidatePassword(string? password, string? confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                throw new Exception("Password and Confirm Password are required");
            }
            if (!password.Equals(confirmPassword))
            {
                throw new Exception("Passwords do not match");
            }
        }

        /// <summary>
        /// Returns created password hash
        /// </summary>
        /// <param name="password">Password string request</param>
        /// <returns>Password Hash</returns>
        private static string CreatePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verify user password
        /// </summary>
        /// <param name="requestedPassword">Input password from user</param>
        /// <param name="applicationUserPasswordHash">Application user password hash with associated email</param>
        private static void VerifyPassword(string? requestedPassword, string applicationUserPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(requestedPassword) || !BCrypt.Net.BCrypt.Verify(requestedPassword, applicationUserPasswordHash))
            {
                throw new Exception("Incorrect email or password.");
            }
        }

        //private static string CreateToken(ApplicationUser )
    }
}
