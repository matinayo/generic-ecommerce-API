using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Application User Models
    /// </summary>
    public class ApplicationUser
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(256)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public bool Active { get; set; } = true;
        /// <summary>
        /// End date if account is locked
        /// </summary>
        public DateTime? LockoutEnd { get; set; }
        public DateTime? UserCreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? DateLastModified { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public ICollection<Roles>? Roles { get; set; }
        public int? RefreshTokenId { get; set; }
        [ForeignKey(nameof(RefreshTokenId))]
        public RefreshToken? RefreshToken { get; set; }
        public bool AccountDeleted { get; set; }
        public DateTime? DateAccountDeleted { get; set; }

        public int? AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        public BaseAddress? Address { get; set; }

        public void DeleteUserAccount()
        {
            AccountDeleted = true;
            DateAccountDeleted = DateTime.UtcNow;
            DateLastModified = DateTime.UtcNow;
            Active = false;
            LockoutEnd = DateTime.UtcNow.AddYears(1000);
        }

        public void FormatUserEmail()
        {
            if(!string.IsNullOrWhiteSpace(Email))
            {
                Email = Email.Trim().ToLower();
            }
        }

        public void Login(string password)
        {
            VerifyPassword(password);
            ValidateAccountStatus(); 
            GenerateRefreshToken();
            LastLoginDate = DateTime.UtcNow;
        }

        public void Register(string? password)
        {
            SetPasswordHash(password);
            FormatUserEmail();
            GenerateRefreshToken();
            LastLoginDate = DateTime.UtcNow;
        }

        /// <summary>
        /// Set created password hash
        /// </summary>
        /// <param name="password">Password string request</param>
        /// <returns>Password Hash</returns>
        private void SetPasswordHash(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Password is required");
            }

            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Verify user password
        /// </summary>
        /// <param name="requestedPassword">Input password from user</param>
        private void VerifyPassword(string? requestedPassword)
        {
            if (string.IsNullOrWhiteSpace(requestedPassword) 
                || !BCrypt.Net.BCrypt.Verify(requestedPassword, PasswordHash))
            {
                throw new Exception("Incorrect email or password.");
            }
        }

        /// <summary>
        /// Validating Account status and preferences 
        /// </summary>
        /// <param name="applicationUser">Application User from db</param>
        private void ValidateAccountStatus()
        {
            bool accountIsInactive = !Active;
            if (LockoutEnd != null)
            {
                DateTime lockoutEnd = LockoutEnd.GetValueOrDefault();
                int result = DateTime.Compare(lockoutEnd, DateTime.UtcNow);
                if (result >= 0)
                {
                    accountIsInactive = true;
                }
            }
            if (accountIsInactive)
            {
                // account is inactive
                throw new Exception("Account is inactive");
            }
        }

        /// <summary>
        /// Generate Refresh Token
        /// </summary>
        /// <returns>New refresh token</returns>
        public void GenerateRefreshToken()
        {
            RefreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                DateExpires = DateTime.UtcNow.AddDays(29),
                DateCreated = DateTime.UtcNow
            };
        }

        public bool ValidateUsingRegex(string emailAddress)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";

            var regex = new Regex(pattern);
            return regex.IsMatch(emailAddress);
        }

    }
}
