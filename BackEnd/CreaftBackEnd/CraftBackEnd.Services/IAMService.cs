using CraftBackEnd.Common.Configs;
using CraftBackEnd.Common.Models;
using CraftBackEnd.Common.Models.IO;
using CraftBackEnd.Common.Models.Type;
using CraftBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CraftBackEnd.Services
{
    public class IAMService : IIAMService
    {
        private const int SaltByteSize = 16;
        private const int HashByteSize = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        private const int Pbkdf2Iterations = 10000;
        private const int IterationIndex = 0;
        private const int Pbkdf2Index = 1;
        private const int SaltIndex = 2;

        private readonly string AccessKey;
        private readonly AuthOptions _authOptions;
        private readonly IDatabaseContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ErrorMessages _errorMessages;
        private readonly IValidationService _validationService;

        protected ILogger Logger { get; }

        public IAMService(IDatabaseContext dbContext,
            IValidationService validationService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<AuthOptions> authOptions,
            IOptionsSnapshot<ErrorMessages> errorMessages,
            ILoggerFactory loggerFactory) {
            _dbContext = dbContext;
            _validationService = validationService;
            _httpContextAccessor = httpContextAccessor;
            _authOptions = authOptions.Value;
            _errorMessages = errorMessages.Value;

            Logger = loggerFactory.CreateLogger($"{GetType().Namespace}.{GetType().Name}");
            AccessKey = _authOptions.Secret;
        }

        public int GetReferenceId() {
            var claim = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(u => u.Type == ClaimTypes.Name);

            return claim == null ? 0 : int.Parse(claim.Value);
        }

        public async Task<IList<UserRole>> GetUserRolesAsync(int userId) {
            var roles = new List<UserRole> {  };
            var user = await _dbContext.User.FindAsync(userId);

            if (user is null)
                return roles;

            roles.Add(UserRole.User);
            if (user.MemberType == MemberType.Premium) roles.Add(UserRole.Premium);

            return roles;
        }

        public async Task<LoginResult> LoginAsync(string userName, string password) {
            try {
                Logger.LogInformation("Entered Login method");
                
                var results = new LoginResult {
                    User = await _dbContext.User.SingleOrDefaultAsync<User>(u => u.Email.Equals(userName) && !u.IsDeleted)
                };

                if (results.User == null) _validationService.ThrowValidationErrors(_errorMessages.GeneralError);
                if (!results.User.IsEmailVerified) _validationService.ThrowValidationErrors(_errorMessages.EmailNotVerified);

                var auth = _dbContext.Auth.SingleOrDefault(a => a.CredRef.Equals(results.User.CredRef));

                if (!ValidatePassword(password, auth.Password, auth.Salt)) _validationService.ThrowValidationErrors(_errorMessages.InvalidLogin);

                Logger.LogDebug($"User: '{userName}' was authenticated");
                results.Token = GenerateJwtToken(results.User.Id.ToString());

                return results;
            } catch (Exception e) {
                Logger.LogError(e, e.Message);
                return null;
            }
        }

        public async Task<bool> CreateAccountAsync(User newUser) {
            var user = await _dbContext.User.SingleOrDefaultAsync(u => u.Email.Equals(newUser.Email) && !u.IsDeleted);

            if (user != null) _validationService.ThrowValidationErrors(_errorMessages.EmailExists); // user account exists already

            var brandNewUser = newUser;
            try {
                brandNewUser.CredRef = CreateUserCredentials(newUser.Password);
                brandNewUser.CreatedAt = DateTimeOffset.Now;
                brandNewUser.UpdatedAt = DateTimeOffset.Now;
                brandNewUser.IsActive = true;
                brandNewUser.IsEmailVerified = true;

                _dbContext.User.Add(brandNewUser);
                _dbContext.SaveChanges();
            } catch (Exception e) {
                Logger.LogError(e, e.Message);
                return false;
            }

            return true;
        }

        public async Task<string> IsAuthenticatedAsync() {
            var userId = GetReferenceId();

            if (userId == 0)
                return "NA";

            var user = await _dbContext.User.FindAsync(userId);
            if (!user.IsActive || user.IsDeleted || !user.IsEmailVerified)
                return "NA";

            return GenerateJwtToken(userId.ToString());
        }

        private bool ValidatePassword(string password, string validHash, string salt) {
            var encoding = new UTF8Encoding();
            var saltByte = Convert.FromBase64String(salt);
            var validHashByte = encoding.GetBytes(validHash);

            var testHash = Convert.ToBase64String(GetPbkdf2Bytes(password, saltByte, Pbkdf2Iterations, HashByteSize));
            var hashedHash = HashHmac256(Pbkdf2Iterations + ":" + testHash);
            var hashedHashByte = encoding.GetBytes(hashedHash);

            return hashedHashByte.SequenceEqual(validHashByte) && hashedHashByte.Length == validHashByte.Length;
        }

        private string GenerateJwtToken(string claimName) {
            // generating jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authOptions.TokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, claimName),
                    new Claim(ClaimTypes.NameIdentifier, claimName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string HashHmac256(string message) {
            var encoding = new UTF8Encoding();
            var keyBytes = encoding.GetBytes(AccessKey);
            var messageBytes = encoding.GetBytes(message);
            var cryptographer = new HMACSHA256(keyBytes);

            var bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes) {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }

        private static string Pbkdf2Hash(string password) {
            var cryptoProvider = new RNGCryptoServiceProvider();
            var salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" + Convert.ToBase64String(hash) + ":" + Convert.ToBase64String(salt);
        }

        private Guid CreateUserCredentials(string password) {
            var guid = Guid.NewGuid();
            var getHash = Pbkdf2Hash(password).Split(new string[] { ":" }, StringSplitOptions.None);
            var hashedHash = HashHmac256(getHash[IterationIndex] + ":" + getHash[Pbkdf2Index]);
            try {
                var authenticationContext = new Auth {
                    Password = hashedHash,
                    Salt = getHash[SaltIndex],
                    CredRef = guid
                };

                _dbContext.Auth.Add(authenticationContext);
            } catch (Exception e) {
                Logger.LogError(e, e.Message);
                return Guid.Empty;
            }

            return guid;
        }
    }
}
